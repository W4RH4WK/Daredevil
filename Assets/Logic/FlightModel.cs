using UnityEngine;
using UnityEngine.Assertions;

public class FlightModel : MonoBehaviour
{
    public struct InputData
    {
        public float Pitch;
        public float Roll;
        public float Yaw;

        public float Throttle;

        public bool FocusMode;
        public bool StrafeMode;
        public bool HighGTurnMode;
    }

    public InputData Input;

    public Controls Controls { get; private set; }

    public FlightModelParameters FlightModelParams { get; private set; }

    // Change in rotation expressed in Euler angles.
    public Vector3 DeltaRotation { get; private set; }

    public Vector3 Velocity { get; private set; }

    public float Speed => Velocity.magnitude;

    float Thrust;

    public bool Stalling { get; private set; } = false;

    float StallingDurationLeft = 0.0f;

    void Awake()
    {
        Controls = new Controls();
        Controls.Enable();

        FlightModelParams = GetComponentInChildren<FlightModelParameters>();
        Assert.IsNotNull(FlightModelParams);

        Thrust = FlightModelParams.BaseThrust;
        Velocity = Thrust * transform.forward;
    }

    void Update()
    {
        UpdateInput();

        UpdateStalling();

        if (!Stalling)
        {
            UpdateRotation();

            UpdateVelocity();
        }
    }

    void UpdateInput()
    {
        Input.Pitch = Controls.Flight.Pitch.ReadValue<float>();

        Input.Roll = Controls.Flight.Roll.ReadValue<float>();

        var yawLeft = Controls.Flight.YawLeft.ReadValue<float>();
        var yawRight = Controls.Flight.YawRight.ReadValue<float>();
        Input.Yaw = yawRight - yawLeft;

        var acceleration = Controls.Flight.Accelerate.ReadValue<float>();
        var deceleration = Controls.Flight.Decelerate.ReadValue<float>();
        Input.Throttle = acceleration - deceleration;

        Input.FocusMode = yawLeft > 0.9f && yawRight > 0.9f;
        if (Input.FocusMode)
        {
            Input.Yaw = -Input.Roll;
            Input.Roll = 0.0f;
        }

        Input.StrafeMode = acceleration > 0.9f && deceleration > 0.9f;

        Input.HighGTurnMode = !Input.StrafeMode && deceleration > 0.9f && Mathf.Abs(Input.Pitch) > 0.78f;
    }

    void UpdateStalling()
    {
        if (!Stalling && Speed < FlightModelParams.StallingAttackSpeed)
        {
            Stalling = true;
            StallingDurationLeft = FlightModelParams.StallingMinimumDuration;

            Thrust *= FlightModelParams.StallingThrustCutFactor;
        }

        if (Stalling)
        {
            var newRotation = Quaternion.LookRotation(Vector3.down, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, FlightModelParams.StallingRotationRate * Time.deltaTime);

            Velocity += FlightModelParams.StallingGravity * Vector3.down * Time.deltaTime;
            transform.position += Velocity * Time.deltaTime;

            StallingDurationLeft -= Time.deltaTime;
        }

        if (Stalling && Speed > FlightModelParams.StallingReleaseSpeed && StallingDurationLeft <= 0.0f)
            Stalling = false;
    }

    void UpdateRotation()
    {
        var pitchRate = Input.Pitch < 0.0f ? FlightModelParams.PitchUpRate : FlightModelParams.PitchDownRate;
        var rates = new Vector3(pitchRate, FlightModelParams.YawRate, FlightModelParams.RollRate);

        if (Input.FocusMode)
            rates = FlightModelParams.FocusModeRotationRate * Vector3.one;
        else if (Input.StrafeMode)
            rates *= FlightModelParams.StrafeRotationalResponseRateModifier;
        else if (Input.HighGTurnMode)
            rates.x *= FlightModelParams.HighGTurnPitchRateModifier;

        var mobility = 1.0f - Speed / FlightModelParams.MaxSpeed / FlightModelParams.Mobility;

        var newDeltaRotation = mobility * Vector3.Scale(rates, new Vector3(Input.Pitch, Input.Yaw, Input.Roll));
        DeltaRotation = Vector3.Lerp(DeltaRotation, newDeltaRotation, FlightModelParams.RotationalResponseRate * Time.deltaTime);

        transform.Rotate(DeltaRotation * Time.deltaTime);

        // Simulate rotation induced by lift when banking.
        {
            var lateralDrift = FlightModelParams.BankingDriftRate * Mathf.Sin(-transform.eulerAngles.z * Mathf.Deg2Rad) * Time.deltaTime;
            transform.Rotate(0.0f, lateralDrift, 0.0f, Space.World);

            var liftLoss = FlightModelParams.BankingLiftLossRate * Mathf.Sin(0.5f * transform.eulerAngles.z * Mathf.Deg2Rad) * Time.deltaTime;
            liftLoss *= FlightModelParams.BankingLiftLossSpeedImpact * (1.0f - Speed / FlightModelParams.MaxSpeed);
            transform.rotation *= Quaternion.Euler(-liftLoss, 0.0f, 0.0f);
        }
    }

    void UpdateVelocity()
    {
        var factor = Input.Throttle > 0.0f ? FlightModelParams.Acceleration : FlightModelParams.Deceleration;
        var thrust = factor * Input.Throttle;

        if (Input.HighGTurnMode)
            thrust *= FlightModelParams.HighGTurnDecelerationModifier;

        var newThrust = FlightModelParams.BaseThrust + thrust;
        Thrust = Mathf.Lerp(Thrust, newThrust, FlightModelParams.ThrustResponseRate * Time.deltaTime);

        // Simulate speed up / slow down induced by gravity when pitching.
        var gravitationalThrust = FlightModelParams.FlightGravity * Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad);

        var responseRate = FlightModelParams.BaseResponseRate;

        if (Input.StrafeMode)
            responseRate = FlightModelParams.StrafeResponseRate;

        var newVelocity = (Thrust + gravitationalThrust) * transform.forward;
        Velocity = Vector3.Lerp(Velocity, newVelocity, responseRate * Time.deltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, FlightModelParams.MaxSpeed);

        transform.position += Velocity * Time.deltaTime;
    }
}

using UnityEngine;
using UnityEngine.Assertions;

public class FlightModel : MonoBehaviour
{
    Controls Controls;

    FlightModelParameters FlightModelParams;

    // Change in rotation expressed in Euler angles.
    Vector3 DeltaRotation;

    Vector3 Velocity;

    public float Speed => Velocity.magnitude;

    public float Altitude => transform.position.y;

    float Thrust;

    public bool FocusMode { get; private set; } = false;

    public bool StrafeMode { get; private set; } = false;

    public bool HighGTurnMode { get; private set; } = false;

    //public bool Stalling => StallingDurationLeft > 0.0f;

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
        UpdateStalling();

        if (!Stalling)
        {
            UpdateFocusMode();

            UpdateStrafeMode();

            UpdateHighGTurnMode();

            UpdateRotation();

            UpdateVelocity();
        }
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
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, FlightModelParams.StallingRotationRate * Time.deltaTime);

            Velocity += FlightModelParams.StallingGravity * Vector3.down * Time.deltaTime;
            transform.localPosition += Velocity * Time.deltaTime;

            StallingDurationLeft -= Time.deltaTime;
        }

        if (Stalling && Speed > FlightModelParams.StallingReleaseSpeed && StallingDurationLeft <= 0.0f)
            Stalling = false;
    }

    void UpdateFocusMode()
    {
        var yawInputLeft = Controls.Flight.YawLeft.ReadValue<float>();
        var yawInputRight = Controls.Flight.YawRight.ReadValue<float>();

        FocusMode = yawInputLeft > 0.9f && yawInputRight > 0.9f;
    }

    void UpdateStrafeMode()
    {
        var accelerationInput = Controls.Flight.Accelerate.ReadValue<float>();
        var decelerationInput = Controls.Flight.Decelerate.ReadValue<float>();

        StrafeMode = accelerationInput > 0.9f && decelerationInput > 0.9f;
    }

    void UpdateHighGTurnMode()
    {
        var pitchInput = Controls.Flight.Pitch.ReadValue<float>();
        var decelerationInput = Controls.Flight.Decelerate.ReadValue<float>();

        HighGTurnMode = !StrafeMode && decelerationInput > 0.9f && (pitchInput < -0.78f || pitchInput > 0.78f);
    }

    void UpdateRotation()
    {
        var pitchInput = Controls.Flight.Pitch.ReadValue<float>();
        var rollInput = Controls.Flight.Roll.ReadValue<float>();
        var yawInputLeft = Controls.Flight.YawLeft.ReadValue<float>();
        var yawInputRight = Controls.Flight.YawRight.ReadValue<float>();
        var yawInput = yawInputRight - yawInputLeft;

        var pitchRate = pitchInput < 0.0f ? FlightModelParams.PitchUpRate : FlightModelParams.PitchDownRate;
        var rates = new Vector3(pitchRate, FlightModelParams.YawRate, FlightModelParams.RollRate);

        if (FocusMode)
        {
            rates = FlightModelParams.FocusModeRotationRate * Vector3.one;

            yawInput = -rollInput;
            rollInput = 0.0f;
        }
        else if (StrafeMode)
        {
            rates *= FlightModelParams.StrafeRotationalResponseRateModifier;
        }
        else if (HighGTurnMode)
        {
            rates.x *= FlightModelParams.HighGTurnPitchRateModifier;
        }

        var mobility = 1.0f - Speed / FlightModelParams.MaxSpeed / FlightModelParams.Mobility;

        var newDeltaRotation = mobility * Vector3.Scale(rates, new Vector3(pitchInput, yawInput, rollInput));
        DeltaRotation = Vector3.Lerp(DeltaRotation, newDeltaRotation, FlightModelParams.RotationalResponseRate * Time.deltaTime);

        transform.localRotation *= Quaternion.Euler(DeltaRotation * Time.deltaTime);
    }

    void UpdateVelocity()
    {
        var acceleration = FlightModelParams.Acceleration * Controls.Flight.Accelerate.ReadValue<float>();
        var deceleration = FlightModelParams.Deceleration * Controls.Flight.Decelerate.ReadValue<float>();

        if (HighGTurnMode)
            deceleration *= FlightModelParams.HighGTurnDecelerationModifier;

        var newThrust = FlightModelParams.BaseThrust + acceleration - deceleration;
        Thrust = Mathf.Lerp(Thrust, newThrust, FlightModelParams.ThrustResponseRate * Time.deltaTime);

        // This component simulates gravity according to the planes rotation.
        var gravitationalThrust = FlightModelParams.FlightGravity * Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad);

        var responseRate = FlightModelParams.BaseResponseRate;

        if (StrafeMode)
            responseRate = FlightModelParams.StrafeResponseRate;

        var newVelocity = (Thrust + gravitationalThrust) * transform.forward;
        Velocity = Vector3.Lerp(Velocity, newVelocity, responseRate * Time.deltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, FlightModelParams.MaxSpeed);

        transform.localPosition += Velocity * Time.deltaTime;
    }
}

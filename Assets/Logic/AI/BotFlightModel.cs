using UnityEngine;
using UnityEngine.Assertions;

public class BotFlightModel : MonoBehaviour
{
    public Vector3 Destination;

    public bool AutoLevelRoll;

    Vector3 DestinationPreviousPosition;

    float DestinationSpeed;

    void UpdateDestination()
    {
        DestinationSpeed = Vector3.Distance(Destination, DestinationPreviousPosition) / Time.deltaTime;
        DestinationPreviousPosition = Destination;
    }

    //////////////////////////////////////////////////////////////////////////

    float RollVelocity = 0.0f;
    float PitchVelocity = 0.0f;

    void UpdateRotation()
    {
        var pitchYawRollRate = FlightModelParams.PitchYawRollRate;

        // Even bots can do highG turns!
        if (Throttle < -0.5f)
            pitchYawRollRate.x *= FlightModelParams.HighGTurnPitchRateModifier;

        var toDestination = Destination - transform.position;
        Debug.DrawLine(transform.position, transform.position + toDestination, Color.blue);

        if (Vector3.Angle(transform.forward, toDestination) < 15.0f)
        {
            // direct rotation

            RollVelocity = 0.0f;
            PitchVelocity = 0.0f;

            var up = AutoLevelRoll ? Vector3.up : transform.up;

            var targetRotation = Quaternion.LookRotation(toDestination, up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pitchYawRollRate.x * Time.deltaTime);
        }
        else
        {
            // roll and pitch

            var toDestinationUp = Vector3.ProjectOnPlane(toDestination, transform.forward).normalized;

            Debug.DrawLine(transform.position, transform.position + 5.0f * toDestinationUp, Color.red);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.up, Color.green);

            var roll = Mathf.SmoothDampAngle(0, Vector3.SignedAngle(transform.up, toDestinationUp, transform.forward), ref RollVelocity, 1.0f / FlightModelParams.PitchYawRollResponseRate.z, pitchYawRollRate.z);
            transform.Rotate(Vector3.forward, roll, Space.Self);

            var pitch = Mathf.SmoothDampAngle(0, Vector3.SignedAngle(transform.forward, toDestination, transform.right), ref PitchVelocity, 1.0f / FlightModelParams.PitchYawRollResponseRate.x, pitchYawRollRate.x);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }
    }

    //////////////////////////////////////////////////////////////////////////

    float Throttle;

    void UpdateThrottle()
    {
        Throttle = 0.0f;

        var toDestination = Destination - transform.position;
        var angleToDestination = Vector3.Angle(transform.forward, toDestination);

        var distanceToDestination = Vector3.Distance(transform.position, Destination);
        if (distanceToDestination < 100.0f)
        {
            if (Speed < DestinationSpeed)
                Throttle = 0.3f;
            else
                Throttle = -0.3f;
        }
        else
        {
            if (angleToDestination < 40.0f)
                Throttle = 0.5f;
            else
                Throttle = -0.7f;
        }
    }

    Vector3 Velocity;

    float Speed => Velocity.magnitude;

    float Thrust;

    void UpdateVelocity()
    {
        var thrustFactor = Throttle > 0.0f ? FlightModelParams.Acceleration : FlightModelParams.Deceleration;

        var newThrust = FlightModelParams.BaseThrust + thrustFactor * Throttle;
        Thrust = Mathf.Lerp(Thrust, newThrust, FlightModelParams.ThrustResponseRate * Time.deltaTime);

        var newVelocity = Thrust * transform.forward;
        Velocity = Vector3.Lerp(Velocity, newVelocity, FlightModelParams.VelocityVectorResponseRate * Time.deltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, FlightModelParams.MaxSpeed);

        transform.position += Velocity * Time.deltaTime;
    }

    //////////////////////////////////////////////////////////////////////////

    FlightModelParameters FlightModelParams;

    void Awake()
    {
        FlightModelParams = GetComponentInChildren<FlightModelParameters>();
        Assert.IsNotNull(FlightModelParams);
    }

    void Start()
    {
        Thrust = FlightModelParams.BaseThrust;
        Velocity = Thrust * transform.forward;
    }

    void Update()
    {
        UpdateDestination();

        UpdateThrottle();

        UpdateRotation();

        UpdateVelocity();
    }
}

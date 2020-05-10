using UnityEngine;
using UnityEngine.Assertions;

public class BotFlightModel : MonoBehaviour
{
    //public Waypoint Waypoint;

    //void UpdateWaypoint()
    //{
    //    if (!Waypoint)
    //        return;

    //    if (Vector3.Distance(transform.position, Waypoint.transform.position) <= Waypoint.ReachedDistance && Waypoint.Next)
    //        Waypoint = Waypoint.Next;

    //}

    //////////////////////////////////////////////////////////////////////////

    public Vector3 Destination;

    Vector3 DestinationPreviousPosition;

    float DestinationSpeed;

    void UpdateDestination()
    {
        //if (Waypoint)
        //    Destination = Waypoint.transform.position;

        // TODO: Handle transition between no and some destination.
        DestinationSpeed = Vector3.Distance(Destination, DestinationPreviousPosition) / Time.deltaTime;
        DestinationPreviousPosition = Destination;
    }

    //////////////////////////////////////////////////////////////////////////

    void UpdateRotation()
    {
        var pitchYawRollRate = FlightModelParams.PitchYawRollRate;

        // Even bots can do highG turns!
        if (Throttle < -0.5f)
            pitchYawRollRate *= FlightModelParams.HighGTurnPitchRateModifier;

        var toDestination = Destination - transform.position;
        Debug.DrawLine(transform.position, transform.position + toDestination, Color.blue);

        if (Vector3.Angle(transform.forward, toDestination) < 20.0f)
        {
            // direct rotation
            var targetRotation = Quaternion.LookRotation(toDestination, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pitchYawRollRate.x * Time.deltaTime);
        }
        else
        {
            // roll and pitch
            var toDestinationUp = Vector3.ProjectOnPlane(toDestination, transform.forward).normalized;

            Debug.DrawLine(transform.position, transform.position + 5.0f * toDestinationUp, Color.red);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.up, Color.green);

            var roll = Limit(Vector3.SignedAngle(transform.up, toDestinationUp, transform.forward), pitchYawRollRate.z * Time.deltaTime);
            transform.Rotate(Vector3.forward, roll, Space.Self);

            var pitch = Limit(Vector3.SignedAngle(transform.forward, toDestination, transform.right), pitchYawRollRate.x * Time.deltaTime);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }
    }

    static float Limit(float value, float limit) => Mathf.Clamp(value, -limit, limit);

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
            if (angleToDestination < 30.0f)
                Throttle = 0.5f;
            else if (angleToDestination > 80.0f)
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
        //UpdateWaypoint();

        UpdateDestination();

        UpdateThrottle();

        UpdateRotation();

        UpdateVelocity();
    }
}

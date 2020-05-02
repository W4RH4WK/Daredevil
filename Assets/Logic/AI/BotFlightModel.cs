using UnityEngine;
using UnityEngine.Assertions;

public class BotFlightModel : MonoBehaviour
{
    public Waypoint Waypoint;

    void UpdateWaypoint()
    {
        if (!Waypoint)
            return;

        if (Vector3.Distance(transform.position, Waypoint.transform.position) <= Waypoint.ReachedDistance && Waypoint.Next)
            Waypoint = Waypoint.Next;

        Destination = Waypoint.transform.position;
    }

    //////////////////////////////////////////////////////////////////////////

    Vector3 Destination;

    void UpdateRotation()
    {
        var toDestination = Destination - transform.position;
        Debug.DrawLine(transform.position, transform.position + toDestination, Color.blue);

        if (Vector3.Angle(transform.forward, toDestination) < 20.0f)
        {
            // direct rotation

            Throttle = 0.5f;

            var targetRotation = Quaternion.LookRotation(toDestination, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, FlightModelParams.PitchYawRollRate.x * Time.deltaTime);
        }
        else
        {
            // roll and pitch

            Throttle = -0.5f;

            var toDestinationUp = Vector3.ProjectOnPlane(toDestination, transform.forward).normalized;

            Debug.DrawLine(transform.position, transform.position + 5.0f * toDestinationUp, Color.red);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + 5.0f * transform.up, Color.green);

            var roll = Limit(Vector3.SignedAngle(transform.up, toDestinationUp, transform.forward), FlightModelParams.PitchYawRollRate.z * Time.deltaTime);
            transform.Rotate(Vector3.forward, roll, Space.Self);

            var pitch = Limit(Vector3.SignedAngle(transform.forward, toDestination, transform.right), FlightModelParams.PitchYawRollRate.x * Time.deltaTime);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }
    }

    static float Limit(float value, float limit) => Mathf.Clamp(value, -limit, limit);

    //////////////////////////////////////////////////////////////////////////

    float Throttle;
    float Thrust;

    Vector3 Velocity;

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
        UpdateWaypoint();

        UpdateRotation();

        UpdateVelocity();
    }
}

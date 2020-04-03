using UnityEngine;

public class FlightControlSurface : MonoBehaviour
{
    public enum FcsOrientation { Horizontal, Vertical }

    public enum FcsSide { Left, Right }

    public FcsOrientation Orientation = FcsOrientation.Horizontal;

    public FcsSide Side = FcsSide.Left;

    public float Factor = 40.0f;

    public float Rate = 60.0f;

    FlightModel FlightModel;

    Quaternion InitialRotation;

    Quaternion TargetRotation;

    void Awake()
    {
        FlightModel = GetComponentInParent<FlightModel>();

        InitialRotation = transform.localRotation;
    }

    void Update()
    {
        if (!FlightModel)
            return;

        var newTargetRotation = Vector3.zero;

        if (Orientation == FcsOrientation.Horizontal)
        {
            var rollInput = FlightModel.Input.Roll;

            if (Side == FcsSide.Right)
                rollInput *= -1.0f;

            newTargetRotation.x = 0.5f * Factor * (FlightModel.Input.Pitch + rollInput);
        }
        else if (Orientation == FcsOrientation.Vertical)
        {
            newTargetRotation.y = Factor * -FlightModel.Input.Yaw;
        }

        TargetRotation = Quaternion.Lerp(TargetRotation, Quaternion.Euler(newTargetRotation), Rate * Time.deltaTime);

        transform.localRotation = InitialRotation * TargetRotation;
    }
}

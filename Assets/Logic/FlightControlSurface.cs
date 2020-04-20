using UnityEngine;

public class FlightControlSurface : MonoBehaviour
{
    public enum FcsOrientation { Horizontal, Vertical }

    public enum FcsSide { Left, Right }

    public FcsOrientation Orientation = FcsOrientation.Horizontal;

    public FcsSide Side = FcsSide.Left;

    public float Factor = 40.0f;

    public float Rate = 60.0f;

    Controls Controls;

    Quaternion InitialRotation;

    Quaternion TargetRotation;

    void Awake()
    {
        InitialRotation = transform.localRotation;
    }

    void Start()
    {
        Controls = GetComponentInParent<Controls>();
    }

    void Update()
    {
        if (!Controls)
            return;

        var newTargetRotation = Vector3.zero;

        if (Orientation == FcsOrientation.Horizontal)
        {
            var rollInput = Controls.PitchYawRoll.z;

            if (Side == FcsSide.Right)
                rollInput *= -1.0f;

            newTargetRotation.x = 0.5f * Factor * (-Controls.PitchYawRoll.x + rollInput);
        }
        else if (Orientation == FcsOrientation.Vertical)
        {
            newTargetRotation.y = Factor * -Controls.PitchYawRoll.y;
        }

        TargetRotation = Quaternion.Lerp(TargetRotation, Quaternion.Euler(newTargetRotation), Rate * Time.deltaTime);

        transform.localRotation = InitialRotation * TargetRotation;
    }
}

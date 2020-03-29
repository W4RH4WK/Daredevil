using UnityEngine;
using UnityEngine.Assertions;

public class FlightCamera : MonoBehaviour
{
    public FlightModel FlightModel;

    public Vector3 Offset = new Vector3(0.0f, 0.8f, -4.7f);

    public float Rate = 5.0f;

    public float LookRate = 5.0f;

    public float SpeedFactor = 2.0f;

    public float FocusModeFovModifier = 0.7f;

    public float StrafeModeFovModifier = 1.2f;

    public float TargetFovSmoothTime = 0.2f;

    public float StallingRateModifier = 0.5f;

    public float StrafeModeRateModifier = 0.4f;

    //////////////////////////////////////////////////////////////////////////

    Camera Camera;

    Vector3 TargetPosition;
    Quaternion TargetRotation;

    void Awake()
    {
        Assert.IsNotNull(FlightModel);

        Camera = GetComponent<Camera>();
        Assert.IsNotNull(Camera);

        BaseFov = Camera.fieldOfView;

        TargetPosition = FlightModel.transform.rotation * Offset;
        TargetRotation = FlightModel.transform.rotation;
    }

    void Update()
    {
        var rate = Rate;
        if (FlightModel.Stalling)
            rate *= StallingRateModifier;
        else if (FlightModel.StrafeMode)
            rate *= StrafeModeRateModifier;

        var SpeedOffset = new Vector3(0.0f, 0.0f, -SpeedFactor * (2.0f * FlightModel.Speed / FlightModel.FlightModelParams.MaxSpeed - 1.0f));

        var newTargetPosition = FlightModel.transform.rotation * (Offset + SpeedOffset);
        TargetPosition = Vector3.Slerp(TargetPosition, newTargetPosition, rate * Time.deltaTime);

        var newTargetRotation = FlightModel.transform.rotation;
        TargetRotation = Quaternion.Slerp(TargetRotation, newTargetRotation, rate * Time.deltaTime);

        UpdateLook();

        transform.position = LookRotation * TargetPosition + FlightModel.transform.position;
        transform.rotation = LookRotation * TargetRotation;

        UpdateFov();
    }

    //////////////////////////////////////////////////////////////////////////

    Quaternion LookRotation;

    void UpdateLook()
    {
        var lookInput = FlightModel.Controls.Flight.Look.ReadValue<Vector2>();
        LookRotation = Quaternion.Slerp(LookRotation, Quaternion.Euler(90.0f * lookInput.y, 180.0f * lookInput.x, 0.0f), LookRate * Time.deltaTime);
    }

    //////////////////////////////////////////////////////////////////////////

    float BaseFov;
    float TargetFov;
    float TargetFovVelocity;

    void UpdateFov()
    {
        TargetFov = BaseFov;

        if (FlightModel.FocusMode)
            TargetFov *= FocusModeFovModifier;
        else if (FlightModel.StrafeMode)
            TargetFov *= StrafeModeFovModifier;

        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, TargetFov, ref TargetFovVelocity, TargetFovSmoothTime);
    }
}

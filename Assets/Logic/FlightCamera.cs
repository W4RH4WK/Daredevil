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

    Quaternion FlightRotation;

    void Awake()
    {
        Assert.IsNotNull(FlightModel);

        Camera = GetComponent<Camera>();
        Assert.IsNotNull(Camera);

        BaseFov = Camera.fieldOfView;

        FlightRotation = FlightModel.transform.rotation;
    }

    void Update()
    {
        UpdateLook();

        UpdateFov();

        var rate = Rate;
        if (FlightModel.Stalling)
            rate *= StallingRateModifier;
        else if (FlightModel.Input.StrafeMode)
            rate *= StrafeModeRateModifier;

        FlightRotation = Quaternion.Slerp(FlightRotation, FlightModel.transform.rotation, rate * Time.deltaTime);

        var SpeedOffset = SpeedFactor * (2.0f * FlightModel.Speed / FlightModel.FlightModelParams.MaxSpeed - 1.0f) * Vector3.back;

        transform.position = FlightRotation * LookRotation * (Offset + SpeedOffset) + FlightModel.transform.position;
        transform.rotation = FlightRotation * LookRotation;

    }

    //////////////////////////////////////////////////////////////////////////

    Quaternion LookRotation;

    void UpdateLook()
    {
        var lookInput = FlightModel.Controls.Flight.Look.ReadValue<Vector2>();

        var newLookRotation = Quaternion.Euler(90.0f * lookInput.y, 180.0f * lookInput.x, 0.0f);
        LookRotation = Quaternion.Slerp(LookRotation, newLookRotation, LookRate * Time.deltaTime);
    }

    //////////////////////////////////////////////////////////////////////////

    float BaseFov;
    float TargetFov;
    float TargetFovVelocity;

    void UpdateFov()
    {
        TargetFov = BaseFov;

        if (FlightModel.Input.FocusMode)
            TargetFov *= FocusModeFovModifier;
        else if (FlightModel.Input.StrafeMode)
            TargetFov *= StrafeModeFovModifier;

        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, TargetFov, ref TargetFovVelocity, TargetFovSmoothTime);
    }
}

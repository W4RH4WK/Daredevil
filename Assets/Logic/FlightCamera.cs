using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Camera))]
public class FlightCamera : MonoBehaviour
{
    public Vector3 BaseOffset = new Vector3(0.0f, 0.8f, -4.7f);

    public float LookingAtTargetOffsetDistance = -8.0f;

    public float TargetOffsetSmoothTime = 0.2f;

    public float Rate = 5.0f;

    public float LookRate = 5.0f;

    public float SpeedFactor = 1.5f;

    public float FocusModeFovModifier = 0.7f;

    public float StrafeModeFovModifier = 1.2f;

    public float TargetFovSmoothTime = 0.2f;

    public float StallingRateModifier = 0.5f;

    public float StrafeModeRateModifier = 0.4f;

    //////////////////////////////////////////////////////////////////////////

    Quaternion LookRotation = Quaternion.identity;

    void UpdateLook()
    {
        if (IsLookingAtTarget)
        {
            LookRotation = Quaternion.identity;
            return;
        }

        var lookInput = Controls.Look;

        var newLookRotation = Quaternion.Euler(-90.0f * lookInput.y, 180.0f * lookInput.x, 0.0f);
        LookRotation = Quaternion.Slerp(LookRotation, newLookRotation, LookRate * Time.deltaTime);
    }

    //////////////////////////////////////////////////////////////////////////

    float BaseFov;
    float TargetFovVelocity;

    void UpdateFov()
    {
        var targetFov = BaseFov;

        if (Controls.FocusMode)
            targetFov *= FocusModeFovModifier;
        else if (Controls.StrafeMode)
            targetFov *= StrafeModeFovModifier;

        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, targetFov, ref TargetFovVelocity, TargetFovSmoothTime);
    }

    //////////////////////////////////////////////////////////////////////////

    Vector3 Offset;
    Vector3 TargetOffsetVelocity;

    void UpdateOffset()
    {
        var targetOffset = BaseOffset;

        if (IsLookingAtTarget)
            targetOffset.z = LookingAtTargetOffsetDistance;

        // speed impact
        targetOffset += SpeedFactor * (2.0f * FlightModel.Speed / FlightModel.FlightModelParams.MaxSpeed - 1.0f) * Vector3.back;

        Offset = Vector3.SmoothDamp(Offset, targetOffset, ref TargetOffsetVelocity, TargetOffsetSmoothTime);
    }

    //////////////////////////////////////////////////////////////////////////

    Camera Camera;

    Controls Controls;

    FlightModel FlightModel;

    CombatModel CombatModel;

    Quaternion FlightRotation = Quaternion.identity;

    bool IsLookingAtTarget => Controls.LookAtTarget && CombatModel.ActiveTarget;

    void OnEnable()
    {
        Controls = FindObjectOfType<Controls>();

        FlightModel = FindObjectOfType<FlightModel>();

        CombatModel = FindObjectOfType<CombatModel>();
    }

    void Start()
    {
        Camera = GetComponent<Camera>();
        Assert.IsNotNull(Camera);

        BaseFov = Camera.fieldOfView;
        Offset = BaseOffset;

        if (FlightModel)
            FlightRotation = FlightModel.transform.rotation;
    }

    void Update()
    {
        if (!Controls || !FlightModel || !CombatModel)
        {
            enabled = false;
            return;
        }

        UpdateLook();

        UpdateFov();

        UpdateOffset();

        var rate = Rate;
        if (FlightModel.Stalling)
            rate *= StallingRateModifier;
        else if (Controls.StrafeMode)
            rate *= StrafeModeRateModifier;

        var newFlightRotation = FlightModel.transform.rotation;
        if (IsLookingAtTarget)
        {
            var forward = CombatModel.ActiveTarget.transform.position - transform.position;
            newFlightRotation = Quaternion.LookRotation(forward, FlightModel.transform.up);
        }

        FlightRotation = Quaternion.Slerp(FlightRotation, newFlightRotation, rate * Time.deltaTime);

        transform.position = FlightRotation * LookRotation * Offset + FlightModel.transform.position;
        transform.rotation = FlightRotation * LookRotation;
    }
}

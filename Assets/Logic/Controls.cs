using UnityEngine;

public class Controls : MonoBehaviour
{
    public Vector3 PitchYawRoll { get; private set; }

    public float Throttle { get; private set; }

    public bool FocusMode { get; private set; }

    public bool StrafeMode { get; private set; }

    public bool HighGTurnMode { get; private set; }

    void UpdateMovement()
    {
        var pitch = InputActions.Flight.Pitch.ReadValue<float>();
        var roll = InputActions.Flight.Roll.ReadValue<float>();
        var yawLeft = InputActions.Flight.YawLeft.ReadValue<float>();
        var yawRight = InputActions.Flight.YawRight.ReadValue<float>();
        var yaw = yawRight - yawLeft;
        PitchYawRoll = new Vector3(pitch, yaw, roll);

        var acceleration = InputActions.Flight.Accelerate.ReadValue<float>();
        var deceleration = InputActions.Flight.Decelerate.ReadValue<float>();
        Throttle = acceleration - deceleration;

        FocusMode = yawLeft > 0.9f && yawRight > 0.9f;
        if (FocusMode)
            PitchYawRoll = new Vector3(pitch, -roll, 0.0f);

        //StrafeMode = acceleration > 0.9f && deceleration > 0.9f;

        HighGTurnMode = !StrafeMode && deceleration > 0.9f && Mathf.Abs(pitch) > 0.78f;
    }

    //////////////////////////////////////////////////////////////////////////

    public Vector2 Look { get; private set; }

    void UpdateLook()
    {
        Look = InputActions.Flight.Look.ReadValue<Vector2>();
    }

    //////////////////////////////////////////////////////////////////////////

    public bool NextTarget { get; private set; }

    public bool LookAtTarget { get; private set; }

    float NextTargetHoldTime = 0.0f;

    void UpdateTargetting()
    {
        // reset
        NextTarget = false;

        var nextTargetInput = InputActions.Flight.Target.ReadValue<float>() > 0.9f;

        if (nextTargetInput)
            NextTargetHoldTime += Time.deltaTime;

        LookAtTarget = nextTargetInput && NextTargetHoldTime > HoldTime;

        // trigger next target on button release
        NextTarget = !nextTargetInput && 0.0f < NextTargetHoldTime && NextTargetHoldTime < HoldTime;

        if (!nextTargetInput)
            NextTargetHoldTime = 0.0f;
    }

    //////////////////////////////////////////////////////////////////////////

    public bool Gun { get; private set; }

    public bool Missile { get; private set; }

    bool MissileDown = false;

    void UpdateWeapons()
    {
        Gun = InputActions.Flight.Gun.ReadValue<float>() > 0.9f;

        var missileInput = InputActions.Flight.Missile.ReadValue<float>() > 0.9f;
        Missile = missileInput && !MissileDown;
        MissileDown = missileInput;
    }

    //////////////////////////////////////////////////////////////////////////

    static float HoldTime = 0.3f;

    InputActions InputActions;

    void Awake() => InputActions = new InputActions();

    void OnEnable() => InputActions.Enable();
    void OnDisable() => InputActions.Disable();

    void Update()
    {
        UpdateMovement();
        UpdateLook();
        UpdateTargetting();
        UpdateWeapons();
    }
}

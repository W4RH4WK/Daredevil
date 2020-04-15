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

    void UpdateTargetting()
    {
        NextTarget = InputActions.Flight.Target.triggered;
    }

    //////////////////////////////////////////////////////////////////////////

    InputActions InputActions;

    void Awake() => InputActions = new InputActions();

    void OnEnable() => InputActions.Enable();
    void OnDisable() => InputActions.Disable();

    void Update()
    {
        UpdateMovement();
        UpdateLook();
        UpdateTargetting();
    }
}

using UnityEngine;

public class LockOn : MonoBehaviour
{
    public float LockOnAngle = 25.0f;

    public float LockOnTime = 1.0f;

    public float LockOnTimeElapsed { get; private set; }

    public bool LockingOn => LockOnTimeElapsed > 0.0f && !LockedOn;

    public bool LockedOn => LockOnTimeElapsed >= LockOnTime;

    public Target Target {
        get => _Target;
        set {
            if (value && _Target != value)
            {
                LockOnTimeElapsed = 0.0f;
                value.LockOns.Add(this);
            }
            else if (!value && _Target)
            {
                _Target.LockOns.Remove(this);
            }

            _Target = value;
        }
    }
    Target _Target;

    void OnDisable() => Target = null;

    void Update()
    {
        if (!Target)
            goto Abort;

        var toTarget = Target.transform.position - transform.position;

        var angle = Vector3.Angle(transform.forward, toTarget);
        if (angle > LockOnAngle)
            goto Abort;

        var layers = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(transform.position, toTarget, out RaycastHit hit, toTarget.magnitude, layers))
            goto Abort;

        LockOnTimeElapsed += Time.deltaTime;
        return;

    Abort:
        LockOnTimeElapsed = 0.0f;
        Target = null;
    }
}

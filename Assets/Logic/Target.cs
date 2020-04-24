using UnityEngine;

public class Target : MonoBehaviour
{
    void OnEnable() => TargetManager.Instance.AddTarget(this);

    void OnDisable()
    {
        if (TargetManager.HasInstance)
            TargetManager.Instance.RemoveTarget(this);
    }
}

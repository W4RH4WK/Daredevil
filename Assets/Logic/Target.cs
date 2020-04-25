using UnityEngine;

public enum Affiliation { Alpha, Bravo };

public class Target : MonoBehaviour
{
    public Affiliation Affiliation;

    void OnEnable() => TargetManager.Instance.AddTarget(this);

    void OnDisable()
    {
        if (TargetManager.HasInstance)
            TargetManager.Instance.RemoveTarget(this);
    }
}

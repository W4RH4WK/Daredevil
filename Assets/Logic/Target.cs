using System.Collections.Generic;
using UnityEngine;

public enum Affiliation { Alpha, Bravo };

public class Target : MonoBehaviour
{
    public Affiliation Affiliation;

    public bool HasDifferentAffiliation(Target other) => Affiliation != other.Affiliation;

    //////////////////////////////////////////////////////////////////////////

    public float AngleFrom(Transform other)
    {
        var fromOther = transform.position - other.position;
        return Vector3.Angle(other.forward, fromOther);
    }

    public float DistanceTo(Vector3 position) => Vector3.Distance(transform.position, position);

    //////////////////////////////////////////////////////////////////////////

    public ISet<LockOn> LockOns = new HashSet<LockOn>();

    //////////////////////////////////////////////////////////////////////////

    void OnEnable() => TargetManager.Instance.AddTarget(this);

    void OnDisable()
    {
        if (TargetManager.HasInstance)
            TargetManager.Instance.RemoveTarget(this);
    }
}

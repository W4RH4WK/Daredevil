using System.Collections.Generic;
using UnityEngine;

public enum Affiliation { Alpha, Bravo };

public class Target : MonoBehaviour
{
    public Affiliation Affiliation;

    public bool HasDifferentAffiliation(Target other) => Affiliation != other.Affiliation;

    //////////////////////////////////////////////////////////////////////////

    public float AngleTo(Vector3 position) => Vector3.Angle(transform.position, position);

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

using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class AAAI : MonoBehaviour
{
    public float AttackDistance = 300.0f;

    Target SelfTarget;

    Target Target;

    Gun[] Guns;

    public GameObject Turrent;

    Target FindTarget()
    {
        return TargetManager.Instance.Targets
            .Where(t => t.HasDifferentAffiliation(SelfTarget))
            .Where(t => t.DistanceTo(transform.position) <= AttackDistance)
            .OrderBy(t => t.AngleFrom(transform))
            .FirstOrDefault();
    }

    void Awake()
    {
        SelfTarget = GetComponent<Target>();
        Assert.IsNotNull(SelfTarget);

        Guns = GetComponentsInChildren<Gun>();
    }

    void Update()
    {
        if (!Target)
            Target = FindTarget();

        if (!Target)
            return;

        if (Target.DistanceTo(transform.position) > AttackDistance)
            Target = null;

        if (!Target)
            return;

        Turrent.transform.LookAt(Target.transform.position, transform.up);

        foreach (var gun in Guns)
            gun.Fire();
    }
}

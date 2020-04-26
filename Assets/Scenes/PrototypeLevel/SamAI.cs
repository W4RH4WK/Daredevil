using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class SamAI : MonoBehaviour
{
    public float Cooldown = 5.0f;

    float CooldownLeft = 0.0f;

    Target SelfTarget;

    Target FindTarget()
    {
        return TargetManager.Instance.Targets
            .Where(t => t.HasDifferentAffiliation(SelfTarget))
            .Where(t => t.AngleTo(transform.position) <= LockOn.LockOnAngle)
            .OrderBy(t => t.DistanceTo(transform.position))
            .FirstOrDefault();
    }

    LockOn LockOn;

    MissileLauncher[] Launchers;

    void Awake()
    {
        SelfTarget = GetComponent<Target>();
        Assert.IsNotNull(SelfTarget);

        LockOn = GetComponent<LockOn>();
        Assert.IsNotNull(LockOn);

        Launchers = GetComponentsInChildren<MissileLauncher>();
    }

    void Update()
    {
        CooldownLeft -= Time.deltaTime;
        if (CooldownLeft > 0.0f)
            return;
        else
            CooldownLeft = Cooldown;

        LockOn.Target = FindTarget();
        if (!LockOn.LockedOn)
            return;

        foreach (var launcher in Launchers)
        {
            var missile = launcher.Fire();
            if (missile)
            {
                var toTarget = LockOn.Target.transform.position - transform.position;
                missile.transform.rotation = Quaternion.LookRotation(toTarget, missile.transform.up);
                missile.GetComponent<Missile>().Target = LockOn.Target;
            }
        }
    }
}

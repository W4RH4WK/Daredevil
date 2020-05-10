using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BotCombatModel : MonoBehaviour
{
    public float GunAcceptableAngle = 7.0f;

    public float GunEffectiveRange = 200.0f;

    public float GunFireDelay = 3.0f;
    float GunFireDelayLeft = 0.0f;

    public float GunFireDuration = 0.5f;
    float? GunFireDurationLeft;

    Gun[] Guns;

    void TryFireGuns()
    {
        GunFireDelayLeft -= Time.deltaTime;
        if (GunFireDelayLeft > 0.0f)
            return;

        if (GunFireDurationLeft.HasValue)
        {
            GunFireDurationLeft -= Time.deltaTime;

            if (GunFireDurationLeft > 0.0f)
            {
                foreach (var gun in Guns)
                    gun.Fire();
            }
            else
            {
                GunFireDurationLeft = null;
                GunFireDelayLeft = GunFireDelay;
            }
        }
        else
        {
            var shouldFire = Target.DistanceTo(transform.position) <= GunEffectiveRange
                          && Target.AngleFrom(transform) <= GunAcceptableAngle;

            if (shouldFire)
                GunFireDurationLeft = GunFireDuration;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public float MissileFireDelay = 10.0f;
    float MissileFireDelayLeft = 0.0f;

    MissileLauncher[] MissileLaunchers;

    void TryFireMissiles()
    {
        MissileFireDelayLeft -= Time.deltaTime;
        if (MissileFireDelayLeft > 0.0f)
            return;

        if (!LockOn.LockedOn)
            return;

        foreach (var launcher in MissileLaunchers)
        {
            var missile = launcher.Fire();
            if (missile)
            {
                MissileFireDelayLeft = MissileFireDelay;

                var missileComponent = missile.GetComponent<Missile>();
                missileComponent.Target = LockOn.Target;

                break;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public void FindTarget()
    {
        var potentialTargets = TargetManager.Instance.Targets
            .Where(t => t.HasDifferentAffiliation(SelfTarget))
            .Where(t => t.DistanceTo(transform.position) <= 2000.0f);

        var randIndex = Random.Range(0, potentialTargets.Count());
        Target = potentialTargets.ElementAtOrDefault(randIndex);
    }

    //////////////////////////////////////////////////////////////////////////

    public Target Target;

    LockOn LockOn;

    Target SelfTarget;

    void Awake()
    {
        Guns = GetComponentsInChildren<Gun>();

        MissileLaunchers = GetComponentsInChildren<MissileLauncher>();

        LockOn = GetComponent<LockOn>();
        Assert.IsNotNull(LockOn);

        SelfTarget = GetComponent<Target>();
        Assert.IsNotNull(SelfTarget);
    }

    void Update()
    {
        if (!Target)
            FindTarget();

        if (!Target)
            return;

        LockOn.Target = Target;

        TryFireGuns();

        TryFireMissiles();
    }
}

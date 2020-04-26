using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Controls))]
public class CombatModel : MonoBehaviour
{
    public Target ActiveTarget { get; private set; }

    Queue<Target> RecentActiveTargets = new Queue<Target>();
    float RecentActiveTargetClearTime = 0.0f;

    public Target SelfTarget { get; private set; }

    public void SelectNextActiveTarget()
    {
        var previousActiveTarget = ActiveTarget;

        if (RecentActiveTargetClearTime < Time.time)
        {
            RecentActiveTargets.Clear();

            if (ActiveTarget)
                RecentActiveTargets.Enqueue(ActiveTarget);
        }

        ActiveTarget = null;

        var visibleTargets = TargetManager.Instance.Targets
            .Where(IsInsideViewport)
            .Where(t => t.HasDifferentAffiliation(SelfTarget))
            .OrderBy(DistanceFromViewportCenter);

        foreach (var target in visibleTargets)
        {
            if (!RecentActiveTargets.Contains(target))
            {
                ActiveTarget = target;
                break;
            }
        }

        while (!ActiveTarget && RecentActiveTargets.Count > 0)
        {
            ActiveTarget = RecentActiveTargets.Dequeue();

            if (!visibleTargets.Contains(ActiveTarget))
                ActiveTarget = null;
        }

        if (ActiveTarget)
        {
            RecentActiveTargets.Enqueue(ActiveTarget);
            RecentActiveTargetClearTime = Time.time + 3.0f;

            while (RecentActiveTargets.Count > 5)
                RecentActiveTargets.Dequeue();
        }
    }

    static bool IsInsideViewport(Target target)
    {
        var point = Camera.main.WorldToViewportPoint(target.transform.position);
        return point.x > 0.0f && point.x < 1.0f && point.y > 0.0f && point.y < 1.0f && point.z > 0.0f;
    }

    static float DistanceFromViewportCenter(Target target)
    {
        var pos = Camera.main.WorldToViewportPoint(target.transform.position);
        if (pos.z < 0.0f)
            return Mathf.Infinity;

        pos.z = 0.0f;
        pos -= new Vector3(0.5f, 0.5f);

        return pos.magnitude;
    }

    //////////////////////////////////////////////////////////////////////////

    Gun[] Guns;

    MissileLauncher[] MissileLaunchers;

    void UpdateWeapons()
    {
        if (Controls.Gun)
        {
            foreach (var gun in Guns)
            {
                var bullet = gun.Fire();
                if (bullet)
                {
                    var bulletComponent = bullet.GetComponent<Bullet>();
                    bulletComponent.OnHit = RegisterHit;
                }
            }
        }

        if (Controls.Missile)
        {
            foreach (var launcher in MissileLaunchers)
            {
                var missile = launcher.Fire();
                if (missile)
                {
                    var missileComponent = missile.GetComponent<Missile>();
                    missileComponent.OnHit = RegisterHit;
                    missileComponent.OnMiss = RegisterMiss;

                    if (LockOn.LockedOn)
                        missileComponent.Target = LockOn.Target;

                    break;
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public bool HitRegistered => LastHitFrameCount == Time.frameCount;

    public void RegisterHit() => LastHitFrameCount = Time.frameCount;

    int LastHitFrameCount = 0;

    //////////////////////////////////////////////////////////////////////////

    public bool MissRegistered => LastMissFrameCount == Time.frameCount;

    public void RegisterMiss() => LastMissFrameCount = Time.frameCount;

    int LastMissFrameCount = 0;

    //////////////////////////////////////////////////////////////////////////

    Controls Controls;

    public LockOn LockOn { get; private set; }

    void Start()
    {
        Controls = GetComponent<Controls>();
        Assert.IsNotNull(Controls);

        LockOn = GetComponent<LockOn>();
        Assert.IsNotNull(LockOn);

        SelfTarget = GetComponent<Target>();
        Assert.IsNotNull(SelfTarget);

        Guns = GetComponentsInChildren<Gun>();

        MissileLaunchers = GetComponentsInChildren<MissileLauncher>();
    }

    void Update()
    {
        if (Controls.NextTarget)
            SelectNextActiveTarget();

        LockOn.Target = ActiveTarget;

        UpdateWeapons();
    }
}

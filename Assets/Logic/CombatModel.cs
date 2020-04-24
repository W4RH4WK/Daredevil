using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Controls))]
public class CombatModel : MonoBehaviour
{
    public float TargetRange = 500.0f;

    public IEnumerator<Target> GetTargetsEnumerator() => Targets.GetEnumerator();

    IList<Target> Targets = new List<Target>();

    void ScanForTargets()
    {
        Targets.Clear();
        foreach (var target in FindObjectsOfType<Target>())
        {
            if (target && Vector3.Distance(transform.position, target.transform.position) <= TargetRange)
                Targets.Add(target);
        }

        if (!Targets.Contains(ActiveTarget))
            ActiveTarget = null;
    }

    //////////////////////////////////////////////////////////////////////////

    public Target ActiveTarget { get; private set; }

    Queue<Target> RecentActiveTargets = new Queue<Target>();
    float RecentActiveTargetClearTime = 0.0f;

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

        var visibleTargets = Targets.Where(IsInsideViewport).OrderBy(DistanceFromViewportCenter).ToList<Target>();

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

        if (previousActiveTarget != ActiveTarget)
            LockOnTimeElapsed = 0.0f;
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

    public float LockOnAngle = 25.0f;

    public float LockOnTime = 0.5f;

    public float LockOnTimeElapsed { get; private set; } = 0.0f;

    public bool LockingOn => LockOnTimeElapsed > 0.0f && !LockedOn;

    public bool LockedOn => LockOnTimeElapsed >= LockOnTime;

    void UpdateLockOn()
    {
        if (!ActiveTarget)
        {
            LockOnTimeElapsed = 0.0f;
            return;
        }

        var toTarget = ActiveTarget.transform.position - transform.position;
        var angle = Vector3.Angle(transform.forward, toTarget);

        if (angle <= LockOnAngle)
            LockOnTimeElapsed += Time.deltaTime;
        else
            LockOnTimeElapsed = 0.0f;
    }

    //////////////////////////////////////////////////////////////////////////

    Gun[] Guns;

    MissileLauncher[] MissileLaunchers;

    void UpdateWeapons()
    {
        if (Controls.Gun)
        {
            foreach (var gun in Guns)
                gun.Fire();
        }

        if (Controls.Missile)
        {
            foreach (var launcher in MissileLaunchers)
            {
                var missile = launcher.Fire();
                if (missile)
                {
                    missile.GetComponent<Missile>().Target = ActiveTarget;
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

    Controls Controls;

    void Start()
    {
        Controls = GetComponent<Controls>();
        Assert.IsNotNull(Controls);

        Guns = GetComponentsInChildren<Gun>();

        MissileLaunchers = GetComponentsInChildren<MissileLauncher>();
    }

    void Update()
    {
        ScanForTargets();

        UpdateLockOn();

        if (Controls.NextTarget)
            SelectNextActiveTarget();

        UpdateWeapons();
    }
}

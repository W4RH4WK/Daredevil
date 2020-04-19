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

    public GameObject BulletPrefab;

    public float GunFireRate = 5.0f;

    float GunCooldown = 0.0f;

    void FireBullet()
    {
        var pos = 10.0f * transform.forward + transform.position;
        Instantiate(BulletPrefab, pos, transform.rotation);

        GunCooldown = 1.0f / GunFireRate;
    }

    public GameObject MissilePrefab;

    public float MissileFireRate = 5.0f;

    float MissileCooldown = 0.0f;

    void FireMissile()
    {
        var pos = 10.0f * transform.forward + transform.position;
        var missile = Instantiate(MissilePrefab, pos, transform.rotation);

        if (LockedOn)
            missile.GetComponent<Missile>().Target = ActiveTarget;

        MissileCooldown = 1.0f / MissileFireRate;
    }

    void UpdateWeapons()
    {
        GunCooldown -= Time.deltaTime;
        MissileCooldown -= Time.deltaTime;

        if (Controls.Gun && GunCooldown <= 0.0f)
            FireBullet();

        if (Controls.Missile && MissileCooldown <= 0.0f)
            FireMissile();
    }

    //////////////////////////////////////////////////////////////////////////

    public GameObject ExplosionPrefab;

    Controls Controls;

    void Awake()
    {
        Controls = GetComponent<Controls>();
        Assert.IsNotNull(Controls);
    }

    void Update()
    {
        ScanForTargets();

        UpdateLockOn();

        if (Controls.NextTarget)
            SelectNextActiveTarget();

        UpdateWeapons();
    }

    void OnTriggerEnter()
    {
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

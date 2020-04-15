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

    Controls Controls;

    void Awake()
    {
        Controls = GetComponent<Controls>();
        Assert.IsNotNull(Controls);
    }

    void Update()
    {
        ScanForTargets();

        if (Controls.NextTarget)
            SelectNextActiveTarget();
    }
}

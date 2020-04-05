using System.Collections.Generic;
using UnityEngine;

public class CombatModel : MonoBehaviour
{
    public float TargetRange = 500.0f;

    IList<Target> Targets = new List<Target>();

    public IEnumerator<Target> GetTargetsEnumerator() => Targets.GetEnumerator();

    void Update()
    {
        ScanForTargets();
    }

    void ScanForTargets()
    {
        Targets.Clear();
        foreach (var target in FindObjectsOfType<Target>())
        {
            if (target && Vector3.Distance(transform.position, target.transform.position) <= TargetRange)
                Targets.Add(target);
        }
    }
}

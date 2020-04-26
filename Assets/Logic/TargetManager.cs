using System;
using System.Collections.Generic;
using System.Linq;

public class TargetManager : Manager<TargetManager>
{
    public void AddTarget(Target target)
    {
        RegisteredTargets.Add(target);
        NewTargetEvent?.Invoke(target);
    }

    public void RemoveTarget(Target target)
    {
        RegisteredTargets.Remove(target);
    }

    public IEnumerable<Target> Targets => RegisteredTargets;

    public event Action<Target> NewTargetEvent;

    IList<Target> RegisteredTargets = new List<Target>();

    void Update()
    {
        RegisteredTargets = RegisteredTargets.Where(t => t).ToList();
    }
}

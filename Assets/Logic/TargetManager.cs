using System;
using System.Collections.Generic;
using System.Linq;

public class TargetManager : Manager<TargetManager>
{
    public IList<Target> Targets { get; private set; } = new List<Target>();

    public void AddTarget(Target target)
    {
        Targets.Add(target);
        NewTargetEvent?.Invoke(target);
    }

    public void RemoveTarget(Target target)
    {
        Targets.Remove(target);
    }

    public event Action<Target> NewTargetEvent;

    void Update()
    {
        Targets = Targets.Where(t => t).ToList();
    }
}

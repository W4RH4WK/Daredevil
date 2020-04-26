using System;
using System.Collections.Generic;
using System.Linq;

public class MissileManager : Manager<MissileManager>
{
    public void AddMissile(Missile missile)
    {
        RegisteredMissiles.Add(missile);
        NewMissileEvent?.Invoke(missile);
    }

    public void RemoveMissile(Missile missile)
    {
        RegisteredMissiles.Remove(missile);
    }

    public IEnumerable<Missile> Missiles => RegisteredMissiles;

    public event Action<Missile> NewMissileEvent;

    IList<Missile> RegisteredMissiles = new List<Missile>();

    void Update()
    {
        RegisteredMissiles = RegisteredMissiles.Where(t => t).ToList();
    }
}

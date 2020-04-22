using UnityEngine;

public interface IMortal
{
    void OnDeath();
}

public static class IMortalHelper
{
    public static void TriggerOnDeath(this GameObject gameObject)
    {
        foreach (var mortal in gameObject.GetComponents<IMortal>())
            mortal.OnDeath();
    }

    public static void TriggerOnDeathInChildren(this GameObject gameObject)
    {
        foreach (var mortal in gameObject.GetComponentsInChildren<IMortal>())
            mortal.OnDeath();
    }
}

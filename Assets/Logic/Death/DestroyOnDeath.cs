using UnityEngine;

public class DestroyOnDeath : MonoBehaviour, IMortal
{
    public void OnDeath() => Destroy(gameObject);
}

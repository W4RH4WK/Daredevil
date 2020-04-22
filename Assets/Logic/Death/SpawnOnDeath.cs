using UnityEngine;

public class SpawnOnDeath : MonoBehaviour, IMortal
{
    public GameObject Prefab;

    public void OnDeath() => Instantiate(Prefab, transform.position, transform.rotation);
}

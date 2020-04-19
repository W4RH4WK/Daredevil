using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    void OnTriggerEnter()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

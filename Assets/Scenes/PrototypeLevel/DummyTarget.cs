using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    void OnCollisionEnter()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

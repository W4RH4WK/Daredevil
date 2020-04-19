using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Lifetime = 3.0f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(0.0f, 0.0f, 100.0f);
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime < 0.0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}

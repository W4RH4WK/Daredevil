using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 150.0f;

    public float Lifetime = 3.0f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.rotation * (Speed * Vector3.forward);
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

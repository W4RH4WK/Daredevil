using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 20;

    public float Speed = 150.0f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.rotation * (Speed * Vector3.forward);
    }

    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
            otherHealth.ReceiveDamage(Damage);
    }
}

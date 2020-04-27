using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 20.0f;

    public float Speed = 150.0f;

    public Action OnHit;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.rotation * (Speed * Vector3.forward);
    }

    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
        {
            otherHealth.ReceiveDamage(Damage);
            OnHit?.Invoke();
        }
    }
}

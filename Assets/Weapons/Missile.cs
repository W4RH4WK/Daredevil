using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    public int Damage = 75;

    public float Speed = 300.0f;

    public float Mobility = 28.0f;

    public Target Target;

    public Action OnHit;

    Rigidbody Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(Rigidbody);
    }

    void Start()
    {
        Rigidbody.velocity = transform.rotation * new Vector3(0.0f, 0.0f, Speed);
    }

    void Update()
    {
        if (Target)
        {
            var toTarget = Target.transform.position - transform.position;
            var newRotation = Quaternion.LookRotation(toTarget, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Mobility * Time.deltaTime);

            Rigidbody.velocity = transform.rotation * new Vector3(0.0f, 0.0f, Speed);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
        {
            otherHealth.ReceiveDamage(Damage);
            OnHit();
        }
    }
}

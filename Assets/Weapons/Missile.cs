using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    public int Damage = 75;

    public float Speed = 300.0f;

    public float Mobility = 28.0f;

    public float BreakOffAngle = 60.0f;

    public Target Target {
        get => _Target;
        set {
            if (value)
                value.Missiles.Add(this);
            else if (_Target)
                _Target.Missiles.Remove(this);

            _Target = value;
        }
    }
    Target _Target;

    public Action OnHit;

    public Action OnMiss;

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

    void OnDisable() => Target = null;

    void Update()
    {
        if (!Target)
            return;

        var toTarget = Target.transform.position - transform.position;

        // handle break off
        if (Vector3.Angle(Rigidbody.velocity, toTarget) > BreakOffAngle)
        {
            Target = null;
            OnMiss?.Invoke();
            return;
        }

        var newRotation = Quaternion.LookRotation(toTarget, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Mobility * Time.deltaTime);

        Rigidbody.velocity = transform.rotation * new Vector3(0.0f, 0.0f, Speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
        {
            otherHealth.ReceiveDamage(Damage);
            OnHit?.Invoke();
        }
        else if (Target)
        {
            OnMiss?.Invoke();
        }
    }
}

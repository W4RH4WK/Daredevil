using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    public float Lifetime = 5.0f;

    public float Speed = 300.0f;

    public float Mobility = 10.0f;

    public Target Target;

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
        Lifetime -= Time.deltaTime;
        if (Lifetime < 0.0f)
        {
            Destroy(gameObject);
            return;
        }

        if (Target)
        {
            var toTarget = Target.transform.position - transform.position;
            var newRotation = Quaternion.LookRotation(toTarget, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Mobility * Time.deltaTime);

            //var newRotation = transform.rotation * Quaternion.FromToRotation(transform.forward, toTarget);

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Mobility * Time.deltaTime);
            //transform.rotation = newRotation;

            //transform.rotation *= Quaternion.RotateTowards(transform.rotation, transform.rotation * newRotation, 1.0f * Time.deltaTime);
            //transform.rotation *= Quaternion.Lerp(Quaternion.identity, newRotation, 0.5f);

            Rigidbody.velocity = transform.rotation * new Vector3(0.0f, 0.0f, Speed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

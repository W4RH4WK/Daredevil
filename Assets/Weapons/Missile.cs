using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    public float Lifetime = 5.0f;

    public float Speed = 300.0f;

    public float Mobility = 28.0f;

    public Target Target;

    public GameObject ExplosionPrefab;

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

            Rigidbody.velocity = transform.rotation * new Vector3(0.0f, 0.0f, Speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

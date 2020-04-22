using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 150.0f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.rotation * (Speed * Vector3.forward);
    }
}

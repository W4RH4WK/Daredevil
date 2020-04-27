using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    public float Damage = 1000.0f;

    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
            otherHealth.ReceiveDamage(Damage);
    }
}

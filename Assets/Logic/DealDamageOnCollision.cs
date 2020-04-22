using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    public int Damage = 1000;

    void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<Health>();
        if (otherHealth)
            otherHealth.ReceiveDamage(Damage);
    }
}

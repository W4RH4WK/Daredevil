using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthLeft = 100.0f;

    public void ReceiveDamage(float amount)
    {
        HealthLeft -= amount;
    }

    void Update()
    {
        if (HealthLeft <= 0)
            gameObject.TriggerOnDeathInChildren();
    }
}

using UnityEngine;

public class Health : MonoBehaviour
{
    public int HealthLeft = 100;

    public void ReceiveDamage(int amount)
    {
        HealthLeft -= amount;
    }

    void Update()
    {
        if (HealthLeft <= 0)
            gameObject.TriggerOnDeathInChildren();
    }
}

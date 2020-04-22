using UnityEngine;

public class DeathOnCollision : MonoBehaviour
{
    public bool CollisionEnter = true;

    public bool TriggerEnter = true;

    void OnCollisionEnter()
    {
        if (CollisionEnter)
            gameObject.TriggerOnDeathInChildren();
    }

    void OnTriggerEnter()
    {
        if (TriggerEnter)
            gameObject.TriggerOnDeathInChildren();
    }
}

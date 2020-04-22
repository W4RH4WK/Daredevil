using UnityEngine;

public class DeathAfterTime : MonoBehaviour
{
    public float SecondsLeft = 2.0f;

    void Update()
    {
        SecondsLeft -= Time.deltaTime;

        if (SecondsLeft <= 0.0f)
            gameObject.TriggerOnDeathInChildren();
    }
}

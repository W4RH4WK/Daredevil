using UnityEngine;

public class DeathOnTrigger : MonoBehaviour
{
    void OnTriggerEnter() => gameObject.TriggerOnDeathInChildren();
}

using UnityEngine;

public class DeathOnCollide : MonoBehaviour
{
    void OnCollisionEnter() => gameObject.TriggerOnDeathInChildren();
}

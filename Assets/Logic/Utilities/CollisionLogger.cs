using UnityEngine;

public class CollisionLogger : MonoBehaviour
{
    void OnTriggerEnter() => Debug.Log("Trigger Enter");

    void OnTriggerStay() => Debug.Log("Trigger Stay");

    void OnTriggerExit() => Debug.Log("Trigger Exit");

    void OnCollisionEnter() => Debug.Log("Collision Enter");

    void OnCollisionStay() => Debug.Log("Collision Stay");

    void OnCollisionExit() => Debug.Log("Collision Exit");
}

using UnityEngine;

public class TargetBracket : MonoBehaviour
{
    public Target Target;

    void Start()
    {
        // hide player's target bracket
        if (Target && Target.GetComponent<FlightModel>())
            Destroy(gameObject);
    }

    void Update()
    {
        if (!Target)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = HUD.TargetScreenPosition(Target);
    }
}

using UnityEngine;

public class MissileBracket : MonoBehaviour
{
    public Missile Missile;

    void Update()
    {
        if (!Missile)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = HUD.ScreenPosition(Missile.transform.position);
    }
}

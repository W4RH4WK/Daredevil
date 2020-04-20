using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;

    public float Delay = 2.0f;

    GameObject Current;

    float? Timer = null;

    void Start()
    {
        Current = Instantiate(Prefab, transform);
    }

    void Update()
    {
        if (Current)
            return;

        if (!Timer.HasValue)
            Timer = Delay;

        Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {
            Current = Instantiate(Prefab, transform);
            Timer = null;
        }
    }
}

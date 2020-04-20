using UnityEngine;
using UnityEngine.Assertions;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject CameraPrefab;

    public GameObject HudPrefab;

    public GameObject PlayerPrefab;

    public GameObject Plane;

    float timer = 3.0f;

    void Spawn()
    {
        var player = Instantiate(PlayerPrefab, transform.position, transform.rotation);
        Instantiate(Plane, player.transform);

        // HUD
        {

            var canvas = FindObjectOfType<Canvas>();
            if (canvas)
                Instantiate(HudPrefab, canvas.transform);
            else
                Debug.LogWarning("Canvas missing for HUD");
        }

        // Camera
        {
            if (Camera.main)
                Destroy(Camera.main.gameObject);

            Instantiate(CameraPrefab);
        }
    }

    void Awake()
    {
        Assert.IsNotNull(CameraPrefab);
        Assert.IsNotNull(HudPrefab);
        Assert.IsNotNull(PlayerPrefab);
        Assert.IsNotNull(Plane);
    }

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if (FindObjectOfType<FlightModel>())
            return;

        timer -= Time.deltaTime;
        if (timer > 0.0f)
            return;

        Spawn();
        timer = 3.0f;
    }
}

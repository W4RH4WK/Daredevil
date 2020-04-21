using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject ProjectilePrefab;

    public float FireRate = 1.0f;

    public Vector3 Spread;

    public GameObject Fire()
    {
        if (Cooldown > 0.0f)
            return null;

        Cooldown = 1.0f / FireRate;

        var accuracy = Quaternion.Euler(
            Random.Range(-Spread.x, Spread.x),
            Random.Range(-Spread.y, Spread.y),
            Random.Range(-Spread.z, Spread.z)
        );

        return Instantiate(ProjectilePrefab, transform.position, accuracy * transform.rotation);
    }

    float Cooldown = 0.0f;

    void Update()
    {
        Cooldown -= Time.deltaTime;
    }
}

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

        var projectile = Instantiate(ProjectilePrefab, transform.position, accuracy * transform.rotation);

        // prevent self collision
        {
            var ownCollider = GetComponentInParent<Collider>();
            var projectileCollider = projectile.GetComponent<Collider>();
            if (ownCollider && projectileCollider)
                Physics.IgnoreCollision(ownCollider, projectileCollider);
        }

        return projectile;
    }

    float Cooldown = 0.0f;

    void Update()
    {
        Cooldown -= Time.deltaTime;
    }
}

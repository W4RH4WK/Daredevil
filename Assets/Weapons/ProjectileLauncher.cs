using System;
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
            UnityEngine.Random.Range(-Spread.x, Spread.x),
            UnityEngine.Random.Range(-Spread.y, Spread.y),
            UnityEngine.Random.Range(-Spread.z, Spread.z)
        );

        var projectile = Instantiate(ProjectilePrefab, transform.position, accuracy * transform.rotation);

        // prevent self collision
        {
            var ownCollider = GetComponentInParent<Collider>();
            var projectileCollider = projectile.GetComponent<Collider>();
            if (ownCollider && projectileCollider)
                Physics.IgnoreCollision(ownCollider, projectileCollider);
        }

        if (CombatModel)
        {
            Action onHit = () =>
            {
                if (CombatModel)
                    CombatModel.RegisterHit();
            };

            var bullet = projectile.GetComponent<Bullet>();
            if (bullet)
                bullet.OnHit = onHit;

            var missile = projectile.GetComponent<Missile>();
            if (missile)
                missile.OnHit = onHit;
        }

        return projectile;
    }

    float Cooldown = 0.0f;

    CombatModel CombatModel;

    void Awake()
    {
        CombatModel = GetComponentInParent<CombatModel>();
    }

    void Update()
    {
        Cooldown -= Time.deltaTime;
    }
}

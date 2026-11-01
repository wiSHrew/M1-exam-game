using UnityEngine;
using UnityEngine.InputSystem;

public class shootingscript : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField, Min(0.01f)] private float shotsPerSecond = 5f;
    private float projectileSpeed = 50f;
    [SerializeField, Min(0f)] private float impactForce = 10f;
    [SerializeField, Min(0.01f)] private float bulletSize = 0.2f;
    [SerializeField] private float bulletSpawnUpwardOffset = 0.1f;

    private float nextShotTime;
    private Collider[] shooterColliders;

    private void Awake()
    {
        shooterColliders = GetComponentsInChildren<Collider>();

        if (muzzle == null)
        {
            muzzle = transform;
        }
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.isPressed || Time.time < nextShotTime)
        {
            return;
        }

        Shoot();
        nextShotTime = Time.time + 1f / shotsPerSecond;
    }

    private void Shoot()
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 spawnPosition = muzzle.position + muzzle.up * bulletSpawnUpwardOffset;
        projectile.transform.SetPositionAndRotation(spawnPosition, muzzle.rotation);
        projectile.transform.localScale = Vector3.one * bulletSize;

        SphereCollider sphereCollider = projectile.GetComponent<SphereCollider>();
        sphereCollider.isTrigger = false;

        Rigidbody projectileBody = projectile.AddComponent<Rigidbody>();
        projectileBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        projectileBody.linearVelocity = muzzle.forward * projectileSpeed;

        ProjectileImpactForce impactHandler = projectile.AddComponent<ProjectileImpactForce>();
        impactHandler.Initialize(impactForce);

        Collider[] projectileColliders = projectile.GetComponentsInChildren<Collider>();
        foreach (Collider projectileCollider in projectileColliders)
        {
            foreach (Collider shooterCollider in shooterColliders)
            {
                Physics.IgnoreCollision(projectileCollider, shooterCollider);
            }
        }
    }
}

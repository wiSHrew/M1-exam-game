using UnityEngine;

public class ProjectileImpactForce : MonoBehaviour
{
    private float impactForce;

    public void Initialize(float force)
    {
        impactForce = force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody targetBody = collision.rigidbody;
        if (targetBody != null && !targetBody.isKinematic)
        {
            targetBody.AddForce(transform.forward * impactForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}

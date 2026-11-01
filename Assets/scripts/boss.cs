using UnityEngine;
using TMPro;

public class boss : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Min(0f)] private float sphereSpeed = 10f;
    private float spawnUpwardOffset = 16.6f;
    private float spawnSidewaysOffset = 6.3f;
    [SerializeField] private TMP_Text counterDisplay;
    [SerializeField] private int startingCount = 1000;

    private int remainingCount;

    private void Start()
    {
        remainingCount = startingCount;
        UpdateCounterDisplay();

        if (counterDisplay == null)
        {
            Debug.LogError("Assign a TextMeshPro component to the counter display field.", this);
        }

        if (target == null)
        {
            Debug.LogError("Assign a target object in the Inspector.", this);
            return;
        }

        InvokeRepeating(nameof(ShootSphere), 0f, 1f);
    }

    private void ShootSphere()
    {
        Vector3 spawnPosition = transform.position
            + transform.up * spawnUpwardOffset
            + transform.right * spawnSidewaysOffset;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = spawnPosition;
        sphere.GetComponent<Renderer>().material.color = Color.red;

        Rigidbody body = sphere.AddComponent<Rigidbody>();
        body.useGravity = false;
        body.linearVelocity = (target.position - sphere.transform.position).normalized * sphereSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (remainingCount <= 0)
        {
            return;
        }

        remainingCount--;
        UpdateCounterDisplay();

        Debug.Log(
            $"{gameObject.name} collided with {collision.gameObject.name} at {collision.GetContact(0).point}.",
            collision.gameObject);

        if (remainingCount == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateCounterDisplay()
    {
        if (counterDisplay != null)
        {
            counterDisplay.SetText("{0}", remainingCount);
        }
    }
}

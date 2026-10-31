using UnityEngine;

public class patrolmovement : MonoBehaviour
{
    [SerializeField] private Vector2[] patrolCoordinates;
    [SerializeField, Min(0f)] private float movementSpeed = 3f;
    [SerializeField, Min(0f)] private float turnSpeed = 540f;

    private int currentCoordinateIndex;

    private void Start()
    {
        if (patrolCoordinates == null || patrolCoordinates.Length == 0)
        {
            Debug.LogError("Assign at least one patrol coordinate in the Inspector.", this);
        }
    }

    private void Update()
    {
        if (patrolCoordinates == null || patrolCoordinates.Length == 0)
        {
            return;
        }

        Vector2 coordinate = patrolCoordinates[currentCoordinateIndex];
        Vector3 targetPosition = new Vector3(coordinate.x, transform.position.y, coordinate.y);
        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            movementSpeed * Time.deltaTime);

        if ((transform.position - targetPosition).sqrMagnitude <= 0.0001f)
        {
            currentCoordinateIndex = (currentCoordinateIndex + 1) % patrolCoordinates.Length;
        }
    }
}

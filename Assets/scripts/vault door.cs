using UnityEngine;

public class vaultdoor : MonoBehaviour
{
    [SerializeField] private GameObject objectToRemove;

    private void Awake()
    {
        if (objectToRemove == null)
        {
            Debug.LogError("Assign the object to remove in the Inspector.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(objectToRemove);
    }
}

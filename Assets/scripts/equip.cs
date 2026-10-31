using UnityEngine;

public class equip : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform transformToEnable;
    [SerializeField] private Transform transformToDisable;

    public void Interact(GameObject interactor)
    {
        if (transformToDisable != null)
        {
            transformToDisable.gameObject.SetActive(false);
        }

        if (transformToEnable != null)
        {
            transformToEnable.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}

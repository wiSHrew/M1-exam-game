using UnityEngine;
using TMPro;

public class credits : MonoBehaviour
{
    [SerializeField] private TMP_Text textToMove;
    [SerializeField, Min(0f)] private float upwardSpeed = 30f;

    private void Awake()
    {
        if (textToMove == null)
        {
            Debug.LogError("Assign a TextMeshPro component to move.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        textToMove.rectTransform.anchoredPosition += Vector2.up * upwardSpeed * Time.deltaTime;
    }
}

using UnityEngine;

public class sizescaler : MonoBehaviour
{
    [SerializeField, Min(0f)] private float scrollSensitivity = 1f;
    [SerializeField, Min(0.01f)] private float minimumScale = 0.25f;
    [SerializeField, Min(0.01f)] private float maximumScale = 3f;

    private Vector3 initialScale;
    private float scaleMultiplier = 1f;
    private bool scalingEnabled;

    public float CurrentScaleMultiplier => scaleMultiplier;

    private void OnEnable()
    {
        initialScale = transform.localScale;
        scaleMultiplier = 1f;
    }

    private void Update()
    {
        if (!scalingEnabled)
        {
            return;
        }

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Approximately(scroll, 0f))
        {
            return;
        }

        scaleMultiplier = Mathf.Clamp(
            scaleMultiplier * (1f + scroll * scrollSensitivity),
            minimumScale,
            maximumScale);
        transform.localScale = initialScale * scaleMultiplier;
    }

    public void SetScalingEnabled(bool enabled)
    {
        scalingEnabled = enabled;
    }
}

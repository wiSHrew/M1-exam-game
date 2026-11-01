using UnityEngine;
using TMPro;
using System.Collections;

public class subtitles : MonoBehaviour
{
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField, TextArea] private string textOnEnter;
    private float secondsPerCharacter = 0.05f;

    private Coroutine revealCoroutine;

    private void Start()
    {
        if (subtitleText != null)
        {
            subtitleText.text = string.Empty;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (subtitleText == null)
        {
            Debug.LogError("Assign a TextMeshPro component to the subtitle text field.", this);
            return;
        }

        if (revealCoroutine != null)
        {
            StopCoroutine(revealCoroutine);
        }

        revealCoroutine = StartCoroutine(RevealText());
    }

    private IEnumerator RevealText()
    {
        subtitleText.text = string.Empty;

        for (int characterCount = 1; characterCount <= textOnEnter.Length; characterCount++)
        {
            subtitleText.text = textOnEnter.Substring(0, characterCount);
            if (characterCount < textOnEnter.Length)
            {
                yield return new WaitForSeconds(secondsPerCharacter);
            }
        }

        yield return new WaitForSeconds(1f);
        subtitleText.text = string.Empty;
        revealCoroutine = null;
    }
}

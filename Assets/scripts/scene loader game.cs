using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloadergame : MonoBehaviour
{
    private const float LoadDelay = 3f;

    [SerializeField] private GameObject objectToWatch;
    [SerializeField] private string sceneName;

    private bool isLoading;

    private void Awake()
    {
        if (objectToWatch == null)
        {
            Debug.LogError("Assign the GameObject to watch in the Inspector.", this);
        }
    }

    private void Update()
    {
        if (isLoading || objectToWatch == null || objectToWatch.activeInHierarchy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Assign the scene name to load in the Inspector.", this);
            return;
        }

        isLoading = true;
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(LoadDelay);
        SceneManager.LoadScene(sceneName);
    }
}

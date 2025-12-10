using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;
    private SceneFadeInOut sceneFadeInOut;

    [Header("Loading Values")]
    [Tooltip("The minimum load time duration that occurs between 2 different scenes")]
    [SerializeField] private float minLoadTime = 2.5f;
    [Tooltip("Delays the scene fade in after the scene has loaded")]
    [SerializeField] private float delayTime = 0.2f;
    private float timeToFade = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        sceneFadeInOut = GetComponent<SceneFadeInOut>();
        if (sceneFadeInOut == null)
            Debug.LogError("Error: SceneSwapManager does not have a SceneFadeInOut component. Please add a SceneFadeInOut component.");
        else 
            timeToFade = sceneFadeInOut.GetTimeToFade();
    
        if (loadingScreen == null)
            Debug.LogError("LevelManager's loadingScreen is null. Please assign a reference.");

        if (progressBar == null)
            Debug.LogError("LevelManager's progressBar is null. Please assign a reference.");
    }

    public void LoadSceneWithProgressBar(string sceneName)
    {
        StartCoroutine(LoadSceneProgressBarOperation(sceneName));
    }

    public void LoadSceneWithoutProgressBar(string sceneName)
    {
        StartCoroutine(LoadSceneOperation(sceneName));
    }

    private IEnumerator LoadSceneProgressBarOperation(string sceneName)
    {
        SceneFadeOut();
        yield return new WaitForSecondsRealtime(timeToFade);

        // Time scale check to make sure timeScale resets propery before every scene
        if (Time.timeScale != 1)
            Time.timeScale = 1;
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        progressBar.fillAmount = 0;

        float startTime = Time.time;
        float targetProgress = 0.9f; // 0.9 is when Unity considers the loading of a scene to be finished
        float progressSpeed = targetProgress / minLoadTime;

        while (scene.progress < targetProgress)
        {
            progressBar.fillAmount = Mathf.MoveTowards(
                progressBar.fillAmount, 
                targetProgress, 
                progressSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Calculate remaining time to meet minimum load time
        float elapsedTime = Time.time - startTime;
        float remainingTime = minLoadTime - elapsedTime;

        if (remainingTime > 0)
        {
            // Continue increasing the progress bar to simulate loading
            while (remainingTime > 0 && progressBar.fillAmount < 1f)
            {
                progressBar.fillAmount = Mathf.MoveTowards(
                    progressBar.fillAmount, 
                    1f, 
                    progressSpeed * Time.deltaTime
                );

                remainingTime -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(remainingTime);
        }

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);

        yield return new WaitForSecondsRealtime(delayTime);
        SceneFadeIn();
    }

    private IEnumerator LoadSceneOperation(string sceneName)
    {
        SceneFadeOut();
        yield return new WaitForSecondsRealtime(timeToFade);

        if (Time.timeScale != 1)
            Time.timeScale = 1;

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSecondsRealtime(delayTime + 0.25f);
        SceneFadeIn();
    }

    private void SceneFadeOut()
    {
        sceneFadeInOut.FadeOut();
    }

    private void SceneFadeIn()
    {
        sceneFadeInOut.FadeIn();
    }
    
}

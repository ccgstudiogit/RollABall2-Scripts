using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    [Tooltip("The name of scene this button should load into")]
    [SerializeField] private string sceneName;
    [Tooltip("Recommended to disable this setting when loading into MainMenu")]
    [SerializeField] private bool includeLoadingScreen = true;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        if (sceneName == null || SceneSwapManager.instance == null)
        {
            Debug.LogWarning($"{name}'s sceneName is blank || SceneSwapManager.instance == null. Unable to load a scene.");
            return;
        }
        else if (includeLoadingScreen)
            SceneSwapManager.instance.LoadSceneWithProgressBar(sceneName);
        else
            SceneSwapManager.instance.LoadSceneWithoutProgressBar(sceneName);
    }

    public string GetSceneName()
    {
        return sceneName;
    }
}

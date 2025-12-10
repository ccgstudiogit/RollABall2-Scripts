using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ReloadSceneButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReloadCurrentScene);
    }

    public void ReloadCurrentScene()
    {
        if (SceneSwapManager.instance != null)
            SceneSwapManager.instance.LoadSceneWithoutProgressBar(SceneManager.GetActiveScene().name);
    }
}

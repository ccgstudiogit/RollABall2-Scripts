using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(Button))]
public class QuitGameButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject quittingGameWindow;
    private RectTransform quittingGameWindowRect;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        if (quittingGameWindow != null)
            quittingGameWindow.SetActive(true);

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

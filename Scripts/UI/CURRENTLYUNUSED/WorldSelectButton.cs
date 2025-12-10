using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WorldSelectButton : MonoBehaviour
{
    [Tooltip("The level selection screen that should be displayed after the user enters this world")]
    [SerializeField] private GameObject levelSelectionScreen;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(EnableLevelSelectionScreen);

        if (levelSelectionScreen == null)
            Debug.LogWarning($"{name}'s levelSelectionScreen null. Please assign a reference.");
    }

    public void EnableLevelSelectionScreen()
    {
        if (levelSelectionScreen != null)
            levelSelectionScreen.SetActive(true);
    }
}

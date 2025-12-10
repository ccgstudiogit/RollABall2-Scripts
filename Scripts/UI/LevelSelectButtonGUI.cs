using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LoadSceneButton), typeof(Image), typeof(Button))]
public class LevelSelectButtonGUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite levelLockedBackground;
    [SerializeField] private Sprite levelUnlockedBackground;
    [SerializeField] private GameObject lockedSymbol;
    [SerializeField] private GameObject buttonText;
    private Image image;
    private LoadSceneButton loadSceneButton;
    private Button button;

    [Header("Unlocking Requirement")]
    [Tooltip("What level needs to be reached and beaten in order to unlock this level")]
    [SerializeField] private int needLevelBeaten;
    [Tooltip("This variable can be manually set in order to override needLevelBeaten")]
    [SerializeField] private bool autoLevelUnlocked;
    private bool levelUnlocked;

    private void Awake()
    {
        image = GetComponent<Image>();
        loadSceneButton = GetComponent<LoadSceneButton>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        UpdateUI();
        
        ResetLevelReachedButton.OnUpdateUI += UpdateUI;
    }

    private void OnDisable()
    {
        ResetLevelReachedButton.OnUpdateUI -= UpdateUI;
    }

    private void UpdateUI()
    {
        DetermineIfLevelIsUnlocked();

        if (!levelUnlocked)
            DisplayLockedLevel();
        else
            DisplayUnlockedLevel();
    }

    private void DetermineIfLevelIsUnlocked()
    {
        if (PlayerPrefs.GetInt("LevelReached") >= needLevelBeaten)
            levelUnlocked = true;
        else if (autoLevelUnlocked)
            levelUnlocked = true;
        else
            levelUnlocked = false;
    }

    private void DisplayLockedLevel()
    {
        if (levelLockedBackground != null)
            image.sprite = levelLockedBackground;

        if (lockedSymbol != null)
            lockedSymbol.SetActive(true);

        if (buttonText != null)
            buttonText.SetActive(false);

        button.interactable = false;
    }

    private void DisplayUnlockedLevel()
    {
        if (levelUnlockedBackground != null)
            image.sprite = levelUnlockedBackground;

        if (lockedSymbol != null)
            lockedSymbol.SetActive(false);

        if (buttonText != null)
            buttonText.SetActive(true);

        button.interactable = true;
    }
}

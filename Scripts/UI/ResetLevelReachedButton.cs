using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetLevelReachedButton : MonoBehaviour
{
    public static event Action OnUpdateUI;

    [SerializeField] private int setLevelReachedTo = 0;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(ChangeLevelReached);
    }

    private void ChangeLevelReached()
    {
        Debug.Log("Changing level reached. . .");
        Debug.Log("LevelReached was: " + PlayerPrefs.GetInt("LevelReached"));

        PlayerPrefs.SetInt("LevelReached", setLevelReachedTo);

        Debug.Log("LevelReached is now: " + PlayerPrefs.GetInt("LevelReached"));

        OnUpdateUI?.Invoke();
    }
}

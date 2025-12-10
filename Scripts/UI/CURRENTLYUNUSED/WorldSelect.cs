using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;

public class WorldSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] worldSelectButtons;
    [Tooltip("Which world will be displayed when World Select Screen first opens")]
    [SerializeField] private int startingIndex = 0;
    private int currentWorld;

    private void Start()
    {
        if (worldSelectButtons.Length == 0)
        {
            Debug.LogWarning("WorldSelect worldSelectButtons length is equal to 0. Please assign references.");
            return;
        }

        currentWorld = startingIndex;
        GetCurrentWorld();
    }

    public void NextOption()
    {
        currentWorld = currentWorld + 1;
        
        if (currentWorld == worldSelectButtons.Length)
            currentWorld = 0;

        GetCurrentWorld();
    }

    public void PreviousOption()
    {
        currentWorld = currentWorld - 1;
        
        if (currentWorld < 0)
            currentWorld = worldSelectButtons.Length - 1;

        GetCurrentWorld();
    }

    public void SelectCurrentWorld()
    {
        Debug.Log("Selected world: " + worldSelectButtons[currentWorld].name);
    }

    private void GetCurrentWorld()
    {
        for (int i = 0; i < worldSelectButtons.Length; i++)
        {
            if (i == currentWorld)
                worldSelectButtons[i].SetActive(true);
            else
                worldSelectButtons[i].SetActive(false);
        }
    }
}

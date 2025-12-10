using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Image[] tabButtons;
    [SerializeField] private int startingTabID = 0;

    [Header("Tab Colors")]
    [SerializeField] private Color inactiveTabColor, activeTabColor;

    private void Start()
    {
        SwitchToTab(startingTabID);
    }

    public void SwitchToTab(int tabID)
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        tabs[tabID].SetActive(true);

        foreach (Image image in tabButtons)
        {
            image.color = inactiveTabColor;
        }

        tabButtons[tabID].color = activeTabColor;
    }
}

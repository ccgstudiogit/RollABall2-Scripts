using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : View
{
    [SerializeField] private Button backButton;

    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}

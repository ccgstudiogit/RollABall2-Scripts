using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenView : View
{
    [SerializeField] private Button playGameButton;

    public override void Initialize()
    {
        playGameButton.onClick.AddListener(() => ViewManager.Show<LevelSelectionView>());
    }
}

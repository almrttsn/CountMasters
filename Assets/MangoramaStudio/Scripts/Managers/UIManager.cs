using MangoramaStudio.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class UIManager : BaseManager
{
    public InGamePanel _inGamePanel;
    public FinishPanel _finishPanel;

    private List<UIPanel> UIPanels;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        UIPanels = new List<UIPanel> {_finishPanel, _inGamePanel };

        UIPanels.ForEach(x =>
        {
            x.Initialize(this);
            x.gameObject.SetActive(false);
        });
        _inGamePanel.ShowPanel();
    }
}
using UnityEngine.UI;
using UnityEngine;
using Utils.Singleton;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] private Button b_closeGeneral;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        b_closeGeneral.onClick.AddListener(delegate { _uiPanelsManager.ControlAlarmPanel(false); ControlStateButtons(false); });
    }

    private void ControlStateButtons(bool activated)
    {
        b_closeGeneral.interactable = activated;
    }
}

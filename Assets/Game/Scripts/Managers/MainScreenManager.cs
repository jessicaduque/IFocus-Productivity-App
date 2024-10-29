using UnityEngine;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] private Button b_alarm, b_computer;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        b_alarm.onClick.AddListener(delegate { _uiPanelsManager.ControlAlarmPanel(true); });
        b_computer.onClick.AddListener(delegate { _uiPanelsManager.ControlComputerScreenPanel(true); });
    }
    
}

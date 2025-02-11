using UnityEngine.UI;
using UnityEngine;
using Utils.Singleton;

public class ComputerScreenManager : Singleton<ComputerScreenManager>
{
    [SerializeField] private Button b_close, b_todoList, b_studyTopics, b_statistics, b_music;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    private new void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        b_close.onClick.AddListener(delegate { _uiPanelsManager.ControlComputerScreenPanel(false);  });
        b_todoList.onClick.AddListener(delegate { _uiPanelsManager.ControlTodoListPanel(true); });
        b_studyTopics.onClick.AddListener(delegate { _uiPanelsManager.ControlStudyTopicsPanel(true); });
        b_statistics.onClick.AddListener(delegate { _uiPanelsManager.ControlStatisticsPanel(true); });
        //b_music.onClick.AddListener(delegate { _uiPanelsManager.ControlMusicPanel(true); });
    }

    public void ControlStateButtons(bool activated)
    {
        b_close.enabled = activated;
        b_todoList.enabled = activated;
        b_studyTopics.enabled = activated;
        b_statistics.enabled = activated;
        //b_music.interactable = activated;
    }
}

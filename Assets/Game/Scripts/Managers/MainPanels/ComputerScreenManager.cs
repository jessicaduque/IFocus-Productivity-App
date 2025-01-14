using UnityEngine.UI;
using UnityEngine;

public class ComputerScreenManager : MonoBehaviour
{
    [SerializeField] private Button b_close, b_todoList, b_studyTopics, b_statistics, b_music;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    private void Awake()
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

    private void ControlStateButtons(bool activated)
    {
        b_close.interactable = activated;
        b_todoList.interactable = activated;
        b_studyTopics.interactable = activated;
        b_statistics.interactable = activated;
        //b_music.interactable = activated;
    }
}

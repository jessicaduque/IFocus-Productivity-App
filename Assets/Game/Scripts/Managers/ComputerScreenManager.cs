using System;
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
        b_close.onClick.AddListener(delegate { _uiPanelsManager.ControlComputerScreenPanel(false); ControlStateButtons(false); });
        b_todoList.onClick.AddListener(delegate { _uiPanelsManager.ControlTodoListPanel(true); ControlStateButtons(false);});
        b_studyTopics.onClick.AddListener(delegate { _uiPanelsManager.ControlStudyTopicsPanel(true); ControlStateButtons(false);});
        b_statistics.onClick.AddListener(delegate { _uiPanelsManager.ControlStatisticsPanel(true); ControlStateButtons(false);});
        b_music.onClick.AddListener(delegate { _uiPanelsManager.ControlMusicPanel(true); ControlStateButtons(false);});
    }

    private void ControlStateButtons(bool activated)
    {
        b_close.interactable = activated;
        b_todoList.interactable = activated;
        b_studyTopics.interactable = activated;
        b_statistics.interactable = activated;
        b_music.interactable = activated;
    }
}

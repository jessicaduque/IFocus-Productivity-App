using Utils.Singleton;
using UnityEngine;
using UnityEngine.Events;

public class UIPanelsManager : Singleton<UIPanelsManager>
{
    [Header("UI Main Panels")]
    [SerializeField] private GameObject alarmPanel;
    [SerializeField] private GameObject computerScreenPanel;
    [SerializeField] private GameObject todoListPanel;
    [SerializeField] private GameObject studyTopicsPanel;
    [SerializeField] private GameObject statisticsPanel;
    [SerializeField] private GameObject musicPanel;
    
    [Header("UI Warning Panels")]
    [SerializeField] private GameObject deleteTopicWarningPanel;
    
    public UnityAction backToMainPanel, alarmPanelActivated, computerScreenPanelActivated, todoListPanelActivated, 
        studyTopicsPanelActivated, statisticsPanelActivated, musicPanelActivated;
    
    #region Panel Controls
    public void ControlAlarmPanel(bool activate)
    {
        if (activate)
        {
            alarmPanel.SetActive(true);
            alarmPanelActivated?.Invoke();
        }
        else
        {
            alarmPanel.SetActive(false);
            backToMainPanel?.Invoke();
        }
    }
    public void ControlComputerScreenPanel(bool activate)
    {
        if (activate)
        {
            computerScreenPanel.SetActive(true);
            computerScreenPanelActivated?.Invoke();
        }
        else
        {
            computerScreenPanel.SetActive(false);
            backToMainPanel?.Invoke();
        }
    }
    public void ControlTodoListPanel(bool activate)
    {
        if (activate)
        {
            todoListPanel.SetActive(true);
            todoListPanelActivated?.Invoke();
        }
        else
        {
            todoListPanel.SetActive(false);
        }
    }
    public void ControlStudyTopicsPanel(bool activate)
    {
        if (activate)
        {
            studyTopicsPanel.SetActive(true);
            studyTopicsPanelActivated?.Invoke();
        }
        else
        {
            studyTopicsPanel.SetActive(false);
        }
    }
    public void ControlStatisticsPanel(bool activate)
    {
        if (activate)
        {
            statisticsPanel.SetActive(true);
            statisticsPanelActivated?.Invoke();
        }
        else
        {
            statisticsPanel.SetActive(false);
        }
    }
    public void ControlMusicPanel(bool activate)
    {
        if (activate)
        {
            musicPanel.SetActive(true);
            musicPanelActivated?.Invoke();
        }
        else
        {
            musicPanel.SetActive(false);
        }
    }
    #endregion
    
    #region Warning Panel Controls

    public void ControlDeleteTopicWarningPanel(bool activate)
    {
        deleteTopicWarningPanel?.SetActive(activate);
    }
    
    #endregion
}

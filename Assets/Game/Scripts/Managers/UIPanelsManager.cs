using Utils.Singleton;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class UIPanelsManager : Singleton<UIPanelsManager>
{
    [Header("UI Main Panels")]
    [SerializeField] private GameObject alarmPanel;
    [SerializeField] private GameObject computerScreenPanel;
    [SerializeField] private GameObject todoListPanel;
    [SerializeField] private GameObject studyTopicsPanel;
    [SerializeField] private GameObject statisticsPanel;
    
    [Header("AINDA NÃO IMPLEMENTADO")]
    [SerializeField] private GameObject musicPanel;

    [Header("UI Images")] 
    [SerializeField] private GameObject backgroundTransparencyObject;

    private CanvasGroup _backgroundTransparencyCanvasGroup;

    [Header("UI Warning Panels")] 
    [SerializeField] private GameObject blurCamera;
    [Header("Delete Panel")]
    [SerializeField] private GameObject deleteTopicWarningPanel;

    private DeleteTopicWarningManager _deleteTopicWarningManager;

    private float _panelTime => Helpers.panelFadeTime;
    
    public UnityAction BackToMainPanelAction, AlarmPanelActivatedAction, ComputerScreenPanelActivatedAction, TodoListPanelActivatedAction, 
        StudyTopicsPanelActivatedAction, StatisticsPanelActivatedAction, MusicPanelActivatedAction;

    protected override void Awake()
    {
        base.Awake();
        computerScreenPanel.transform.localScale = Vector3.zero;
        
        // Getting private variables
        _backgroundTransparencyCanvasGroup = backgroundTransparencyObject.GetComponent<CanvasGroup>();
        _deleteTopicWarningManager = deleteTopicWarningPanel.GetComponent<DeleteTopicWarningManager>();
    }

    #region Panel Controls
    public void ControlAlarmPanel(bool activate)
    {
        if (activate)
        {
            alarmPanel.SetActive(true);
            AlarmPanelActivatedAction?.Invoke();
        }
        else
        {
            alarmPanel.SetActive(false);
            BackToMainPanelAction?.Invoke();
        }
    }
    public void ControlComputerScreenPanel(bool activate)
    {
        if (activate)
        {
            computerScreenPanel.SetActive(true);
            computerScreenPanel.transform.DOScale(1, _panelTime);
            backgroundTransparencyObject.SetActive(true);
            _backgroundTransparencyCanvasGroup.DOFade(1, _panelTime);
            ComputerScreenPanelActivatedAction?.Invoke();
        }
        else
        {
            computerScreenPanel.transform.DOScale(0, _panelTime).OnComplete(() => computerScreenPanel.SetActive(false));
            backgroundTransparencyObject.SetActive(false);
            _backgroundTransparencyCanvasGroup.DOFade(0, _panelTime);
            BackToMainPanelAction?.Invoke();
        }
    }
    public void ControlTodoListPanel(bool activate)
    {
        if (activate)
        {
            todoListPanel.SetActive(true);
            TodoListPanelActivatedAction?.Invoke();
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
            StudyTopicsPanelActivatedAction?.Invoke();
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
            StatisticsPanelActivatedAction?.Invoke();
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
            MusicPanelActivatedAction?.Invoke();
        }
        else
        {
            musicPanel.SetActive(false);
        }
    }
    #endregion
    
    #region Warning Panel Controls

    public void ControlDeleteTopicWarningPanel(bool activate, string deleteName="")
    {
        if (activate)
        {
            deleteTopicWarningPanel.SetActive(true);
            _deleteTopicWarningManager.SetDeleteTopicName(deleteName);
        }
        else
        {
            _deleteTopicWarningManager.ClosePanel();
        }
        blurCamera.SetActive(activate);
    }
    
    #endregion
}

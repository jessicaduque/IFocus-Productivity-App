using Utils.Singleton;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIPanelsManager : Singleton<UIPanelsManager>
{
    [Header("UI Main Panels")]
    [SerializeField] private GameObject timerPanel;
    private MaximizedTimer _maximizedTimer;
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
    
    [Header("UI Notification Panels")] 
    [Header("Have Water Panel")]
    [SerializeField] private GameObject haveWaterNotificationPanel;
    private WaterPlantsSystemManager _waterPlantSystemManager;
    
    private float _panelTime => Helpers.panelFadeTime;
    
    public UnityAction BackToMainPanelAction, AlarmPanelActivatedAction, ComputerScreenPanelActivatedAction, TodoListPanelActivatedAction, 
        StudyTopicsPanelActivatedAction, StatisticsPanelActivatedAction, MusicPanelActivatedAction;

    protected override void Awake()
    {
        base.Awake();
        computerScreenPanel.transform.localScale = Vector3.zero;
        haveWaterNotificationPanel.transform.localScale = Vector3.zero;
        
        // Getting private variables
        _backgroundTransparencyCanvasGroup = backgroundTransparencyObject.GetComponent<CanvasGroup>();
        _deleteTopicWarningManager = deleteTopicWarningPanel.GetComponent<DeleteTopicWarningManager>();
        _waterPlantSystemManager = haveWaterNotificationPanel.GetComponent<WaterPlantsSystemManager>();
        _maximizedTimer = timerPanel.GetComponent<MaximizedTimer>();
    }

    #region Panel Controls
    public void ControlAlarmPanel(bool activate)
    {
        if (activate)
        {
            timerPanel.SetActive(true);
            AlarmPanelActivatedAction?.Invoke();
        }
        else
        {
            _maximizedTimer.ClosePanel();
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
    
    #region Notification Panel Controls

    public void ControlHaveWaterNotificationPanel(bool activate)
    {
        if (activate)
        {
            haveWaterNotificationPanel.SetActive(true);
            haveWaterNotificationPanel.transform.DOScale(1, _panelTime);
            backgroundTransparencyObject.SetActive(true);
            _backgroundTransparencyCanvasGroup.DOFade(1, _panelTime);
        }
        else
        {
            haveWaterNotificationPanel.transform.DOScale(0, _panelTime).OnComplete(() => haveWaterNotificationPanel.SetActive(false));
            backgroundTransparencyObject.SetActive(false);
            _backgroundTransparencyCanvasGroup.DOFade(0, _panelTime);
        }
    }
    
    #endregion
}

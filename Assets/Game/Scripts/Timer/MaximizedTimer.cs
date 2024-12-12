using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Singleton;
public class MaximizedTimer : Singleton<MaximizedTimer>
{
    [Header("UI Components")]
    [SerializeField] private GameObject setTimerText;
    [SerializeField] private Image uiFill;
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button generalLeaveButton;
    [Header("Input Fields")] 
    [SerializeField] private TMP_InputField hoursInputField;
    [SerializeField] private TMP_InputField minutesInputField;
    [SerializeField] private TMP_InputField secondsInputField;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;

    private float totalSeconds;
    
    public UnityAction playTimerAction, resumeTimerAction, pauseTimerAction, quitTimerAction;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    private TimerManager _timerManager => TimerManager.I;
    

    private new void Awake()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        _timerManager.endTimerAction += InitialUI;
        
        if(_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF)
            InitialUI();
    }

    private void OnDisable()
    {
        _timerManager.endTimerAction -= InitialUI;
    }

    private void SetTotalSeconds()
    {
        totalSeconds = 0;
        _timerManager.SetTotalSeconds(totalSeconds);
        
        if(hoursInputField.text != "" && hoursInputField.text != " ")
            totalSeconds += int.Parse(hoursInputField.text) * 3600;
        
        if(minutesInputField.text != "" && minutesInputField.text != " ")
            totalSeconds += int.Parse(minutesInputField.text) * 60;
        
        if(secondsInputField.text != "" && secondsInputField.text != " ")
            totalSeconds += int.Parse(secondsInputField.text);
        
        totalSeconds += 1;
        
        _timerManager.SetTotalSeconds(totalSeconds);
    }

    private void SetupButtons()
    {
        playButton.onClick.AddListener(Play);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        generalLeaveButton.onClick.AddListener(delegate { _uiPanelsManager.ControlAlarmPanel(false); ControlStateButtons(true); });
    }
    
    #region Unity Callbacks

    private void Play()
    {
        SetTotalSeconds();
        if (_timerManager.GetTotalSeconds() == 1)
            return;
        
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON);
        PlayUI();
        
    }

    private void Pause()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_PAUSED);
        PauseUI();
    }

    private void Resume()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON);
        ResumeUI();
    }
    
    private void Quit()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_OFF);
        InitialUI();
    }
    
    #endregion
    
    #region UI Callbacks

    private void PlayUI()
    {
        hoursInputField.text = "";
        minutesInputField.text = "";
        secondsInputField.text = "";
        
        hoursInputField.gameObject.SetActive(false);
        minutesInputField.gameObject.SetActive(false);
        secondsInputField.gameObject.SetActive(false);
        
        setTimerText.SetActive(false);
        
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void PauseUI()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    private void InitialUI()
    {
        hoursText.text = "0";
        minutesText.text = "00";
        secondsText.text = "00";

        uiFill.fillAmount = 1;
        
        hoursInputField.gameObject.SetActive(true);
        minutesInputField.gameObject.SetActive(true);
        secondsInputField.gameObject.SetActive(true);
        
        setTimerText.SetActive(true);
        
        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    private void ResumeUI()
    {
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
    
    private void ControlStateButtons(bool activated)
    {
        generalLeaveButton.interactable = activated;
        playButton.interactable = activated;
        pauseButton.interactable = activated;
        quitButton.interactable = activated;
        resumeButton.interactable = activated;
    }
    
    #endregion
    
    #region Timer Control

    private void Update()
    {
        if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_ON)
        {
            float secondsLeft = _timerManager.GetSecondsLeft();
            if(secondsLeft < 0) secondsLeft = 0;
            int hours = (int) (secondsLeft / 3600);
            int minutes = (int) ((secondsLeft - hours * 3600) / 60);
            int seconds = (int) (secondsLeft % 60);
            hoursText.text = $"{hours:0}";
            minutesText.text = $"{minutes:00}";
            secondsText.text = $"{seconds:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, totalSeconds, secondsLeft);
        }
    }

    private void ResumeTimer()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON);
    }
    

    #endregion
    
}

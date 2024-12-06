using System.Collections;
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
    [Header("Input Fields")] 
    [SerializeField] private TMP_InputField hoursInputField;
    [SerializeField] private TMP_InputField minutesInputField;
    [SerializeField] private TMP_InputField secondsInputField;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;
        
    private float _totalSeconds;
    private float _secondsLeft;

    public UnityAction playTimerAction, resumeTimerAction, pauseTimerAction, quitTimerAction, endTimerAction;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetTotalSeconds()
    {
        _totalSeconds = 0;
        
        if(hoursInputField.text != "" && hoursInputField.text != " ")
            _totalSeconds += int.Parse(hoursInputField.text) * 3600;
        
        if(minutesInputField.text != "" && minutesInputField.text != " ")
            _totalSeconds += int.Parse(minutesInputField.text) * 60;
        
        if(secondsInputField.text != "" && secondsInputField.text != " ")
            _totalSeconds += int.Parse(secondsInputField.text);
        
        _totalSeconds += 1;
        _secondsLeft = _totalSeconds;
    }

    private void SetupButtons()
    {
        playButton.onClick.AddListener(Play);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }
    
    #region Unity Callbacks

    private void Play()
    {
        SetTotalSeconds();
        if (_totalSeconds == 1)
            return;
        
        BeginTimer(_totalSeconds);
        PlayUI();
        playTimerAction?.Invoke();
        
    }

    private void Pause()
    {
        StopTimer();
        PauseUI();
        pauseTimerAction?.Invoke();
    }

    private void Resume()
    {
        ResumeTimer();
        ResumeUI();
        resumeTimerAction?.Invoke();
    }
    
    private void Quit()
    {
        StopTimer();
        QuitUI();
        quitTimerAction?.Invoke();
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

    private void QuitUI()
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
    
    #endregion
    
    #region Timer Control
    private void BeginTimer (float seconds)
    {
        _secondsLeft = seconds;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_secondsLeft >= 0) 
        {
            int hours = (int) (_secondsLeft / 3600);
            int minutes = (int) ((_secondsLeft - hours * 3600) / 60);
            int seconds = (int) (_secondsLeft % 60);
            hoursText.text = $"{hours:0}";
            minutesText.text = $"{minutes:00}";
            secondsText.text = $"{seconds:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, _totalSeconds, _secondsLeft);
            _secondsLeft -= Time.deltaTime;
            yield return null;
        }
        
        EndTimer();
    }

    private void ResumeTimer()
    {
        BeginTimer(_secondsLeft);
    }

    private void StopTimer()
    {
        StopAllCoroutines();
    }
    
    private void EndTimer()
    {
        QuitUI();
        endTimerAction?.Invoke();
    }

    #endregion
    
    #region Get

    public float GetTimerTotalSeconds()
    {
        return _totalSeconds;
    }
    
    public float GetTimerSecondsLeft()
    {
        return _secondsLeft;
    }
    
    #endregion
}

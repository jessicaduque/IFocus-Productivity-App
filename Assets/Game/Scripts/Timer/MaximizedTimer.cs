using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Singleton; // Script de Singleton criado para herdar e ser chamado com facilidade
public class MaximizedTimer : Singleton<MaximizedTimer>
{
    [Header("UI Components")] // Componentes de UI para controlar o timer maximizado
    [SerializeField] private GameObject setTimerText;
    [SerializeField] private Image uiFill;
    [Header("Buttons")] // Botões do alarme para manipulá-lo
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button generalLeaveButton;
    [Header("Input Fields")] // Input fields do timer para input do usuário e definição do timer
    [SerializeField] private TMP_InputField hoursInputField;
    [SerializeField] private TMP_InputField minutesInputField;
    [SerializeField] private TMP_InputField secondsInputField;
    [Header("Texts")] // Textos que estão no lugar do input field para demonstrar melhor ao usuário
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;

    private float totalSeconds; // Total de segundos do timer quando é definido 

    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I; // Pegar o singleton do UIPanelsManager para controlar o painel de timer maximizado
    private TimerManager _timerManager => TimerManager.I; // Pegar o singleton do TimerManager que controla dados sobre o timer, principalmente considerando quando o painel de TimerMaximizado estará inativo
    
    // Função default do Unity que roda uma única vez quando um objeto ativo está sendo carregado 
    private new void Awake()
    {
        SetupButtons(); // Função para definir o que cada botão faz no início (mantendo assim maior controle dos componentes)
    }
    
    // Função default do Unity que roda toda vez que o objeto ligado ao script é ativado
    private void OnEnable()
    {
        _timerManager.endTimerAction += InitialUI; // Inscreve a função de UI na ação de fim de timer para atualizar a UI ao estado primário dele
        
        if(_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF) // Se o timer estiver no estado desligado, atualiza a UI à seu estado primário
            InitialUI();
    }
    // Função default do Unity que roda toda vez que o objeto ligado ao script é desativado
    private void OnDisable()
    {
        _timerManager.endTimerAction -= InitialUI; // Desinscreve a funçãon de UI inicial à ação de fim de timer para ele não ser chamado  se o painel está desativado
    }
    // Função para setar o total de segundos qunando um timer novo é definido
    private void SetTotalSeconds()
    {
        totalSeconds = 0;
    
        // Cálculo para mostrar as horas faltando ao usuário
        if(hoursInputField.text != "" && hoursInputField.text != " ")
            totalSeconds += int.Parse(hoursInputField.text) * 3600;
        // Cálculo para mostrar os minutos faltando ao usuário
        if(minutesInputField.text != "" && minutesInputField.text != " ")
            totalSeconds += int.Parse(minutesInputField.text) * 60;
        // Cálculo para mostrar os segundos faltando ao usuário
        if(secondsInputField.text != "" && secondsInputField.text != " ")
            totalSeconds += int.Parse(secondsInputField.text);
        
        totalSeconds += 1; // Gambiarra para que quando o timer iniciar, o timer não começar visualmente com um segundo a menos e o usuário estranhar
        
        _timerManager.SetTotalSeconds(totalSeconds); // Seta o total de segundos no TimerManager para controlar as informações de segundos do timer
    }
    // Função para setup inicial dos botões com seus funcionamentos
    private void SetupButtons()
    {
        playButton.onClick.AddListener(Play); // Adiciona o método de play quando o timer é ligado
        pauseButton.onClick.AddListener(Pause); // Adiciona o método de pausa quando o timer é pausado
        resumeButton.onClick.AddListener(Resume); // Adiciona o método de resumir o timer quando é resumido
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

using System.Collections;
using DG.Tweening;
using Game.Scripts.Audio;
using TMPro;
using Unity.VisualScripting; // Biblioteca comum da Unity para manipulação de componentes de UI referentes ao tipo de texto mais atualizado e recomendado para uso
using UnityEngine; // Biblioteca padrão da Unity para manipulação de questões básicas da engine
using UnityEngine.UI; // Biblioteca padrão da Unity para manipulação de componentes de UI
using Sequence = DG.Tweening.Sequence; // Script de Singleton criado para herdar e ser chamado com facilidade
public class MaximizedTimer : Utils.Singleton.Singleton<MaximizedTimer> // Esta classe é um singleton pois apenas um timer maximizado irá existir no projeto, então será mais fácil acessá-lo de outras classes
{
    [Header("UI Components")] // Componentes de UI para controlar o timer maximizado
    [SerializeField] private GameObject setTimerText;
    [SerializeField] private Image uiFill;
    [Header("Buttons")] // Botões do alarme para manipulá-lo
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;
    [Header("Input Fields")] // Input fields do timer para input do usuário e definição do timer
    [SerializeField] private TMP_InputField hoursInputField;
    [SerializeField] private TMP_InputField minutesInputField;
    [SerializeField] private TMP_InputField secondsInputField;
    [Header("Texts")] // Textos que estão no lugar do input field para demonstrar melhor ao usuário
    [SerializeField] private TextMeshProUGUI hoursText;
    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;

    [Header("Animations")] 
    [SerializeField] private Transform mainTimerObjectTransform; 
    [SerializeField] private Transform[] possibleButtonsTransforms = new Transform[4]; 
    private Sequence sequenceOpen, sequenceClose;
    private float _animationsTime => Helpers.panelFadeTime;
    
    private float _totalSeconds; // Total de segundos do timer quando é definido 

    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I; // Pegar o singleton do UIPanelsManager para controlar o painel de timer maximizado
    private TimerManager _timerManager; // Pegar o singleton do TimerManager que controla dados sobre o timer, principalmente considerando quando o painel de TimerMaximizado estará inativo
    private AudioManager _audioManager => AudioManager.I;
    // Método default da Unity que roda uma única vez quando um objeto ativo está sendo carregado 
    private new void Awake()
    {
        _timerManager = FindObjectOfType<TimerManager>();
        SetupButtons(); // Método para definir o que cada botão faz no início (mantendo assim maior controle dos componentes)
    }
    
    // Método default da Unity que roda toda vez que o objeto ligado ao script é ativado
    private void OnEnable()
    {
        _timerManager.endTimerAction += delegate { 
            _audioManager.PlaySfx("timerEnd");
            InitialUI();
        }; // Inscreve a método de UI na ação de fim de timer para atualizar a UI ao estado primário dele e tocar o som de fim de timer

        if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF)// Se o timer estiver no estado desligado, atualiza a UI à seu estado primário
        {
            InitialUI();
        } 
        else if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_PAUSED) // Se o timer estiver no estado pausado, atualiza a UI à seu estado de pausa e a quantidade total de segundos
        {
            _totalSeconds = _timerManager.GetTotalSeconds();
            PauseUI(); 
        }
        else // Se o timer estiver no estado ligado, atualiza a UI à seu estado de ativado e a quantidade total de segundos
        {
            _totalSeconds = _timerManager.GetTotalSeconds();
            PlayUI();
        }

        StartCoroutine(AnimationOpen());
    }
    // Método default da Unity que roda toda vez que o objeto ligado ao script é desativado
    private void OnDisable()
    {
        _timerManager.endTimerAction -= delegate { 
            _audioManager.PlaySfx("timerEnd");
            InitialUI();
        }; // Desinscreve a método de UI inicial à ação de fim de timer para ele não ser chamado  se o painel está desativado
    }
    // Método para setar o total de segundos qunando um timer novo é definido
    private void SetTotalSeconds()
    {
        _totalSeconds = 0;
    
        // Cálculo para mostrar as horas faltando ao usuário
        if(hoursInputField.text != "" && hoursInputField.text != " ")
            _totalSeconds += int.Parse(hoursInputField.text) * 3600;
        // Cálculo para mostrar os minutos faltando ao usuário
        if(minutesInputField.text != "" && minutesInputField.text != " ")
            _totalSeconds += int.Parse(minutesInputField.text) * 60;
        // Cálculo para mostrar os segundos faltando ao usuário
        if(secondsInputField.text != "" && secondsInputField.text != " ")
            _totalSeconds += int.Parse(secondsInputField.text);
        
        _totalSeconds += 1; // Gambiarra para que quando o timer iniciar, o timer não começar visualmente com um segundo a menos e o usuário estranhar
        
        _timerManager.SetTotalSeconds(_totalSeconds); // Seta o total de segundos no TimerManager para controlar as informações de segundos do timer
    }
    // Método para setup inicial dos botões com seus funcionamentos
    private void SetupButtons()
    {
        playButton.onClick.AddListener(Play); // Adiciona o método de play quando o timer é ligado
        pauseButton.onClick.AddListener(Pause); // Adiciona o método de pausa quando o timer é pausado
        resumeButton.onClick.AddListener(Resume); // Adiciona o método de resumir o timer quando é resumido
        quitButton.onClick.AddListener(Quit); // Adiciona o método de cancelar o timer quando é cancelado
        exitButton.onClick.AddListener(delegate { _uiPanelsManager.ControlAlarmPanel(false); });  
    }
    
    #region Unity Callbacks
    // Método para quando timer maximizado começa
    private void Play()
    {
        SetTotalSeconds(); // Pega o total de tempo do timer definido pelo usuário
        if (_timerManager.GetTotalSeconds() == 1) // Considerando aquela gambiarra feita, se for igual a 1, é porque o usuário não inseriu um tempo de timer, e logo nada será inicializado
            return;
        
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON); // O estado do timer é atualizado para ligado
        PlayUI(); // A UI correspondente à quando o timer está ligado é atualizada
        
    }
    // Método para quando o timer maximizado é pausado
    private void Pause()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_PAUSED); // O estado do timer é atualizado para pausado
        PauseUI(); // A UI correspondente à quando o timer é pausado é atualizada
    }
    // Método para quando o timer maximizado estava pausado e é agora resumido
    private void Resume()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON); // O estado do timer é atualizado para ligado
        ResumeUI(); // A UI correspondente à quando o timer sai do modo pausado e é resumido é atualizada
    }
    // Método para quando o timer é cancelado antes do tempo acabar
    private void Quit()
    {
        _timerManager.SetTimerState(TIMER_STATE.TIMER_OFF); // O estado do timer é atualizado para desligado
        InitialUI(); // A UI correspondente à quando o timer está no seu estado inicial é atualizada
    }
    
    #endregion
    
    #region UI Callbacks
    // Método para atualizar a UI do timer maximizado quando ele é setado e ligado
    private void PlayUI()
    {
        uiFill.fillAmount = Mathf.InverseLerp(0, _totalSeconds, _timerManager.GetSecondsLeft());
        hoursInputField.text = "";
        minutesInputField.text = "";
        secondsInputField.text = "";
        
        hoursInputField.gameObject.SetActive(false);
        minutesInputField.gameObject.SetActive(false);
        secondsInputField.gameObject.SetActive(false);
        
        setTimerText.SetActive(false);
        
        playButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
    // Método para atualizar a UI do timer maximizado quando ele é pausado
    private void PauseUI()
    {
        hoursInputField.text = "";
        minutesInputField.text = "";
        secondsInputField.text = "";
        
        hoursInputField.gameObject.SetActive(false);
        minutesInputField.gameObject.SetActive(false);
        secondsInputField.gameObject.SetActive(false);
        
        setTimerText.SetActive(false);
        
        float secondsLeft = _timerManager.GetSecondsLeft();
        if(secondsLeft < 0) secondsLeft = 0;
        int hours = (int) (secondsLeft / 3600);
        int minutes = (int) ((secondsLeft - hours * 3600) / 60);
        int seconds = (int) (secondsLeft % 60);
        hoursText.text = $"{hours:0}";
        minutesText.text = $"{minutes:00}";
        secondsText.text = $"{seconds:00}";
        uiFill.fillAmount = Mathf.InverseLerp(0, _totalSeconds, secondsLeft);
        
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
    // Método para atualizar a UI do timer maximizado quando ele é ainda não foi ligado e aguardo inputs do usuário
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
    // Método para atualizar a UI do timer maximizado quando ele é resumido depois de ter estado pausado
    private void ResumeUI()
    {
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
    // Método para controlar a interação com os botões do timer maximizado
    private void ControlStateButtons(bool activated)
    {
        playButton.enabled = activated;
        pauseButton.enabled = activated;
        quitButton.enabled = activated;
        resumeButton.enabled = activated;
        exitButton.enabled = activated;
    }

    public void ClosePanel()
    {
        AnimationClose();
    }
    
    #endregion
    
    #region Timer Control
    // Método default da Unity que roda a cada frame 
    private void Update()
    {
        if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_ON) // Se o estado do timer for ativado, irá realizar os cálculos do tempo faltando e apresentar estes dados ao usuário através de textos e imagens
        {
            float secondsLeft = _timerManager.GetSecondsLeft();
            if(secondsLeft < 0) secondsLeft = 0;
            int hours = (int) (secondsLeft / 3600);
            int minutes = (int) ((secondsLeft - hours * 3600) / 60);
            int seconds = (int) (secondsLeft % 60);
            hoursText.text = $"{hours:0}";
            minutesText.text = $"{minutes:00}";
            secondsText.text = $"{seconds:00}";
            uiFill.fillAmount = Mathf.InverseLerp(0, _totalSeconds, secondsLeft);
        }
    }

    #endregion
    
    #region Animations

    private IEnumerator AnimationOpen()
    {
        ControlStateButtons(false);
        exitButton.transform.localScale = Vector3.zero;
        mainTimerObjectTransform.localScale = Vector3.one;
        
        yield return new WaitForNextFrameUnit();
        
        sequenceOpen = DOTween.Sequence();
        
        sequenceOpen.Join(mainTimerObjectTransform.DOPunchPosition(Vector3.up * 10, _animationsTime * 3, 5, 0).SetEase(Ease.InOutBounce));
        
        float time = 0;
        for (int indexButton = 0; indexButton < possibleButtonsTransforms.Length; indexButton++)
        {
            if (possibleButtonsTransforms[indexButton].gameObject.activeSelf)
            {
                possibleButtonsTransforms[indexButton].localScale = Vector3.one;
                sequenceOpen.Insert(time, possibleButtonsTransforms[indexButton].DOPunchPosition(Vector3.up * 10, _animationsTime * 2, 5, 0).SetEase(Ease.InOutBounce));
                time += _animationsTime / 2;
            }
        }

        sequenceOpen.Insert(_animationsTime / 2, exitButton.transform.DOScale(1, _animationsTime));

        sequenceOpen.OnComplete(delegate { ControlStateButtons(true); });
    }
    
    private void AnimationClose()
    {
        ControlStateButtons(false);
        sequenceClose = DOTween.Sequence();
        
        sequenceClose.Join(mainTimerObjectTransform.DOScale(0, _animationsTime));
        
        foreach (var buttonTransform in possibleButtonsTransforms)
        {
            if (buttonTransform.gameObject.activeSelf)
            {
                sequenceClose.Join(buttonTransform.DOScale(0, _animationsTime));
            }
        }

        sequenceClose.Join(exitButton.transform.DOScale(0, _animationsTime));

        sequenceClose.OnComplete(delegate {gameObject.SetActive(false);});
    }
    
    #endregion
    
}

using TMPro; // Biblioteca comum da Unity para manipulação de componentes de UI referentes ao tipo de texto mais atualizado e recomendado para uso
using UnityEngine; // Biblioteca padrão da Unity para manipulação de questões básicas da engine
using UnityEngine.UI; // Biblioteca padrão da Unity para manipulação de componentes de UI
using Utils.Singleton; // Script de Singleton criado para herdar e ser chamado com facilidade
public class MaximizedTimer : Singleton<MaximizedTimer> // Esta classe é um singleton pois apenas um timer maximizado irá existir no projeto, então será mais fácil acessá-lo de outras classes
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
    
    // Método default da Unity que roda uma única vez quando um objeto ativo está sendo carregado 
    private new void Awake()
    {
        SetupButtons(); // Método para definir o que cada botão faz no início (mantendo assim maior controle dos componentes)
    }
    
    // Método default da Unity que roda toda vez que o objeto ligado ao script é ativado
    private void OnEnable()
    {
        _timerManager.endTimerAction += InitialUI; // Inscreve a método de UI na ação de fim de timer para atualizar a UI ao estado primário dele
        
        if(_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF) // Se o timer estiver no estado desligado, atualiza a UI à seu estado primário
            InitialUI();
    }
    // Método default da Unity que roda toda vez que o objeto ligado ao script é desativado
    private void OnDisable()
    {
        _timerManager.endTimerAction -= InitialUI; // Desinscreve a método de UI inicial à ação de fim de timer para ele não ser chamado  se o painel está desativado
    }
    // Método para setar o total de segundos qunando um timer novo é definido
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
    // Método para setup inicial dos botões com seus funcionamentos
    private void SetupButtons()
    {
        playButton.onClick.AddListener(Play); // Adiciona o método de play quando o timer é ligado
        pauseButton.onClick.AddListener(Pause); // Adiciona o método de pausa quando o timer é pausado
        resumeButton.onClick.AddListener(Resume); // Adiciona o método de resumir o timer quando é resumido
        quitButton.onClick.AddListener(Quit); // Adiciona o método de cancelar o timer quando é cancelado
        generalLeaveButton.onClick.AddListener(delegate { _uiPanelsManager.ControlAlarmPanel(false); ControlStateButtons(true); }); // Adiciona ações de desligar o painel de alarme maximizado e re-ligar os componentes de botões dele quando é fechado 
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
    // Método para atualizar a UI do timer maximizado quando ele é pausado
    private void PauseUI()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
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
        generalLeaveButton.interactable = activated;
        playButton.interactable = activated;
        pauseButton.interactable = activated;
        quitButton.interactable = activated;
        resumeButton.interactable = activated;
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
            uiFill.fillAmount = Mathf.InverseLerp(0, totalSeconds, secondsLeft);
        }
    }

    #endregion
    
}

using TMPro; // Biblioteca comum da Unity para manipulação de componentes de UI referentes ao tipo de texto mais atualizado e recomendado para uso
using UnityEngine; // Biblioteca padrão da Unity para manipulação de questões básicas da engine
using UnityEngine.UI; // Biblioteca padrão da Unity para manipulação de componentes de UI

public class MinimizedTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _thisText; // Texto para mostrar o tempo faltando do timer 
    [SerializeField] private Image _fillImage; // Imagem que se preenche para mostrar progresso conforme timer vai passando

    private float _totalSeconds; // Para setar sempre corretamente a imagem de progresso, é necessário saber o total de segundos setado originalmente pelo usuário quando um timer é definido
    private TimerManager _timerManager => TimerManager.I; // Pega o singleton do TimerManager para obter informações do timer
    
    // Método default da Unity que roda uma única vez (após o Awake) em um objeto ativo bem antes da função de update começar a rodar a cada frame  
    private void Start()
    {
        if(_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF) // Para confirmar que objeto esteja desligado no começo do jogo caso ele esteja ligado e nenhum timer ativo
            gameObject.SetActive(false);
    }
    // Método default da Unity que toda uma única vez toda vez que o objeto ligado ao script é ativado
    private void OnEnable()
    {
        SetTotalSeconds(); // Seta o total de segundos do timer
    }
    // Método default da Unity que toda uma única vez toda vez que o objeto ligado ao script é desativado
    private void OnDisable()
    {
        _thisText.text = "0:00:00"; // Bota o texto em um estado default
    }
    // Método para setar o total de segundos de um timer que está rodando
    private void SetTotalSeconds()
    {
        _totalSeconds = _timerManager.GetTotalSeconds(); // A informação do total de segundos é pego do TimerManager
    }
    // Método default da Unity que roda a cada frame
    void Update()
    {
        if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_ON) // Checa se o timer está no estado ativado para fazer os cálculos de tempos e apresentar os dados ao usuário
        {
            float secondsLeft = _timerManager.GetSecondsLeft();
            int hours = (int) (secondsLeft / 3600);
            int minutes = (int) ((secondsLeft - hours * 3600) / 60);
            int seconds = (int) (secondsLeft % 60);
            _thisText.text = $"{hours:0}:{minutes:00}:{seconds:00}";
            _fillImage.fillAmount = Mathf.InverseLerp(0, _totalSeconds, secondsLeft);
        }
    }
}

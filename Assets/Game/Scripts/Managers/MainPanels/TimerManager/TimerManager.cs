using System;
using System.Collections;
using UnityEngine; // Biblioteca padrão da Unity para manipulação de questões básicas da engine
using UnityEngine.Events; // Biblioteca padrão da Unity para manipulação de eventos da Unity
//using Utils.Singleton; // Script de Singleton criado para herdar e ser chamado com facilidade
public class TimerManager : MonoBehaviour // Esta classe é um singleton pois serve de controlador principal do timer e é o único a existir no projeto, então será mais fácil acessá-lo de outras classes
{
    private float _totalSeconds; // Total de segundos definido para um timer novo pelo usuário
    private float _secondsLeft; // Quantidade de segundos faltando para o timer terminar

    private TIMER_STATE timerState { get; set; }  = TIMER_STATE.TIMER_OFF; // Estado atual do timer
    public UnityAction endTimerAction; // Ação que será chamada quando o tempo do timer terminar

    [SerializeField] private GameObject minimizedTimer; // Objeto do timer minimizado para poder controlá-lo

    #region Unity Default Methods
    protected new void Awake()
    {
        LoadTimerInfo();
    }

    private void OnApplicationQuit()
    {
        ApplicationQuit();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        ApplicationPause(pauseStatus);
    }
    
    #endregion
    
    #region Timer Control
    // Corrotina para atualizar o timer a cada frame
    private IEnumerator UpdateTimer()
    {
        while (_secondsLeft >= 0) // Corrotina irá rodar até o tempo acabar
        {
            _secondsLeft -= Time.deltaTime; // Descresce do tempo faltando os segundos entre o frame atual e o último
            yield return null; // Retorna nada e volta ao começo do while para checar novamente a condição
        }
        
        SetTimerState(TIMER_STATE.TIMER_OFF); // Seta o estado do timer para desativado quando o timer acabar
        endTimerAction?.Invoke(); // Invoca a ação de fim de timer
    }

    #endregion
    
    #region Timer Save Information

    private void SaveTimerInfo(float secondsTimerLeft, float totalSeconds, bool isPaused)
    {
        PlayerPrefs.SetFloat("SecondsLeftTimer", secondsTimerLeft);
        PlayerPrefs.SetFloat("TotalSecondsTimer", totalSeconds);
        PlayerPrefs.SetString("DatetimeExit", DateTime.Now.ToString());
        PlayerPrefs.SetInt("TimerIsPaused", isPaused ? 1 : 0);
    }

    private void LoadTimerInfo()
    {
        if (PlayerPrefs.HasKey("SecondsLeftTimer"))
        {
            DateTime dateOfExit = DateTime.Parse(PlayerPrefs.GetString("DatetimeExit"));
            var timeSinceExit = DateTime.Now - dateOfExit;
            float secondsLeftExit = PlayerPrefs.GetFloat("SecondsLeftTimer");
            bool isPaused = PlayerPrefs.GetInt("TimerIsPaused") == 1;
            if (isPaused && secondsLeftExit > 0)
            {
                _secondsLeft = secondsLeftExit;
                _totalSeconds = PlayerPrefs.GetFloat("TotalSecondsTimer");
                SetTimerState(TIMER_STATE.TIMER_PAUSED);
            }
            else
            {
                _secondsLeft = secondsLeftExit - (float)timeSinceExit.TotalSeconds;
                if (_secondsLeft <= 0)
                {
                    _secondsLeft = 0;
                    SetTimerState(TIMER_STATE.TIMER_OFF);
                    if (secondsLeftExit != 0)
                    {
                        endTimerAction?.Invoke();
                    }
                }
                else
                {
                    _totalSeconds = PlayerPrefs.GetFloat("TotalSecondsTimer");
                    SetTimerState(TIMER_STATE.TIMER_ON);
                }
            }
        }
    }
    
    #endregion
    
    #region Methods for Application Events

    public void ApplicationQuit()
    {
        SaveTimerInfo((int)_secondsLeft, _totalSeconds, timerState == TIMER_STATE.TIMER_PAUSED);
    }
    
    public void ApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTimerInfo(_secondsLeft, _totalSeconds, timerState == TIMER_STATE.TIMER_PAUSED);
        }
        else
        {
            LoadTimerInfo();
        }
    }
    
    #endregion

    
    
    #region Set
    // Seta o total de segundos do timer 
    public void SetTotalSeconds(float totalSeconds)
    {
        _totalSeconds = totalSeconds;
        _secondsLeft = totalSeconds;
    }
    // Seta o estado do timer
    public void SetTimerState(TIMER_STATE newState)
    {
        timerState = newState;

        switch (newState)
        {
            case TIMER_STATE.TIMER_ON:
                StopAllCoroutines(); // Garante de que não existe já algum timer ativado
                StartCoroutine(UpdateTimer()); // Se timer é ativado, uma corrotina de atualizar o timer é criada
                minimizedTimer?.SetActive(true); // Se timer é ativado, o objeto do timer minimizado é ligado
                break;
            case TIMER_STATE.TIMER_OFF:
                minimizedTimer?.SetActive(false); // Se timer é desastivado, o objeto do timer minimizado é desligado 
                StopAllCoroutines(); // Se timer é desastivado, mata todas as instâncias de corrotina que estão rodando
                break;
            case TIMER_STATE.TIMER_PAUSED:
                StopAllCoroutines(); // Se timer é pausado, mata todas as instâncias de corrotina que estão rodando
                minimizedTimer?.SetActive(true); // Se timer é ativado, o objeto do timer minimizado é ligado
                break;
        }
    }
    
    #endregion
    #region Get
    // Get da quantidade de segundos faltando do timer
    public float GetSecondsLeft()
    {
        return _secondsLeft;
    }
    // Get do total de segundos definido para o timer
    public float GetTotalSeconds()
    {
        return _totalSeconds;
    }
    // Get do estado atual do timer
    public TIMER_STATE GetTimerState()
    {
        return timerState;
    }
    #endregion
}

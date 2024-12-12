using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimizedTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _thisText;
    [SerializeField] private Image _fillImage;

    private float _totalSeconds;
    private TimerManager _timerManager => TimerManager.I;
    
    private void Start()
    {
        // Para confirmar que objeto esteja desligado no começo do jogo caso ele esteja ligado e nenhum timer ativo
        if(_timerManager.GetTimerState() == TIMER_STATE.TIMER_OFF)
            gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        SetTotalSeconds();
    }

    private void OnDisable()
    {
        _thisText.text = "0:00:00";
    }

    private void SetTotalSeconds()
    {
        _totalSeconds = _timerManager.GetTotalSeconds();
    }
    
    void Update()
    {
        if (_timerManager.GetTimerState() == TIMER_STATE.TIMER_ON)
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

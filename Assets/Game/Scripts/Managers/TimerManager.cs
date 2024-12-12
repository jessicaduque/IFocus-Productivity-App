using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : Utils.Singleton.Singleton<TimerManager>
{
    private float _totalSeconds;
    private float _secondsLeft;
    
    public TIMER_STATE timerState = TIMER_STATE.TIMER_OFF;
    public UnityAction endTimerAction;

    [SerializeField] private GameObject minimizedTimer;

    #region Timer Control

    private IEnumerator UpdateTimer()
    {
        while (_secondsLeft >= 0) 
        {
            _secondsLeft -= Time.deltaTime;
            yield return null;
        }
        
        SetTimerState(TIMER_STATE.TIMER_OFF);
        endTimerAction?.Invoke();
    }

    #endregion
    
    #region Set
    public void SetTotalSeconds(float totalSeconds)
    {
        _totalSeconds = totalSeconds;
        _secondsLeft = totalSeconds;
    }

    public void SetTimerState(TIMER_STATE newState)
    {
        timerState = newState;

        switch (newState)
        {
            case TIMER_STATE.TIMER_ON:
                StartCoroutine(UpdateTimer());
                minimizedTimer.SetActive(true);
                break;
            case TIMER_STATE.TIMER_OFF:
                minimizedTimer.SetActive(false);
                StopAllCoroutines();
                break;
            case TIMER_STATE.TIMER_PAUSED:
                StopAllCoroutines();
                break;
        }
    }
    
    #endregion
    #region Get
    public float GetSecondsLeft()
    {
        return _secondsLeft;
    }
    public float GetTotalSeconds()
    {
        return _totalSeconds;
    }
    public TIMER_STATE GetTimerState()
    {
        return timerState;
    }
    #endregion
}

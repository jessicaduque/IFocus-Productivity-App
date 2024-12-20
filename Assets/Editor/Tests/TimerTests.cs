using NUnit.Framework;
using UnityEngine;
using System;

public class TimerTests
{
    private TimerManager _timerManager;

    [SetUp]
    public void Setup()
    {
        // Inicializa uma instância do TimerManager antes de cada teste
        _timerManager = (new GameObject().AddComponent<TimerManager>()).GetComponent<TimerManager>();
        _timerManager.SetTotalSeconds(0); // Reseta o estado inicial do timer
    }

    [Test]
    public void ActiveTimerTimeNotFinishedRecalculation_ApplicationUnpaused()
    {
        // Arrange
        _timerManager.SetTotalSeconds(1500); // 25 minutos
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON);
        _timerManager.SimulateOnApplicationPause(true); // Simula saída do app

        DateTime fakeReturnTime = DateTime.Now;

        // Simula retorno após 5 minutos
        DateTime fakeExitTime = fakeReturnTime.AddMinutes(-5);
        var elapsed = fakeReturnTime - fakeExitTime;
        PlayerPrefs.SetString("DatetimeExit", fakeExitTime.ToString());
        _timerManager.SimulateOnApplicationPause(false); // Simula retorno ao app

        // Act
        float expectedTimeRemaining = 1500 - (float)elapsed.TotalSeconds;

        // Assert
        Assert.AreEqual(expectedTimeRemaining, _timerManager.GetSecondsLeft(), 1.0f);
        Assert.AreEqual(TIMER_STATE.TIMER_ON, _timerManager.GetTimerState());
    }

    [Test]
    public void ActiveTimerTimeEnds_ApplicationUnpaused()
    {
        // Arrange
        _timerManager.SetTotalSeconds(300); // 5 minutos
        _timerManager.SetTimerState(TIMER_STATE.TIMER_ON);
        _timerManager.SimulateOnApplicationPause(true);
        // Simula saída e retorno após 6 minutos
        DateTime fakeExitTime = DateTime.Now.AddMinutes(-6);
        PlayerPrefs.SetString("DatetimeExit", fakeExitTime.ToString());
        _timerManager.SimulateOnApplicationPause(false);

        // Act & Assert
        Assert.AreEqual(0, _timerManager.GetSecondsLeft(), 1.0f);
        Assert.AreEqual(TIMER_STATE.TIMER_OFF, _timerManager.GetTimerState());
    }

    [Test]
    public void PausedTimerTimeSaved_OnApplicationQuit()
    {
        // Arrange
        _timerManager.SetTotalSeconds(600); // 10 minutos
        _timerManager.SetTimerState(TIMER_STATE.TIMER_PAUSED);

        // Act
        _timerManager.SimulateOnApplicationQuit();

        // Assert
        Assert.AreEqual(600, _timerManager.GetSecondsLeft());
        Assert.AreEqual(TIMER_STATE.TIMER_PAUSED, _timerManager.GetTimerState());
    }

    [TearDown]
    public void Teardown()
    {
        // Limpa PlayerPrefs e destrói o objeto após cada teste
        PlayerPrefs.DeleteAll();
        UnityEngine.Object.DestroyImmediate(_timerManager.gameObject);
    }
}
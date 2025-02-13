using System;
using Utils.Singleton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenController : DontDestroySingleton<BlackScreenController>
{
    [SerializeField] private GameObject blackScreen_Panel;
    [SerializeField] private CanvasGroup blackScreen_CanvasGroup;
    [SerializeField] private GameObject intialGamePanel;
    [SerializeField] private CanvasGroup intialGamePanel_CanvasGroup;
    private float _blackFadeTime => Helpers.blackFadeTime;
    public Action gameStartAction;
    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        OnSceneLoaded();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded()
    {
        blackScreen_Panel.SetActive(true);
        intialGamePanel.SetActive(true);
        blackScreen_CanvasGroup.alpha = 1;
        intialGamePanel_CanvasGroup.alpha = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(intialGamePanel_CanvasGroup.DOFade(1, _blackFadeTime));
        sequence.AppendInterval(1.5f);
        sequence.Append(intialGamePanel_CanvasGroup.DOFade(0, _blackFadeTime));
        sequence.OnComplete(delegate
        {
            intialGamePanel.SetActive(false);
            gameStartAction?.Invoke();
            FadeInSceneStart();
        });
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeInSceneStart();
    }

    private void OnDestroy()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Fade in and out

    public void FadeInBlack()
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, _blackFadeTime);
    }

    public void FadeOutBlack()
    {
        blackScreen_CanvasGroup.DOFade(0, _blackFadeTime);
        blackScreen_Panel.SetActive(false);
    }

    #endregion

    #region Fades with scenes
    public void FadeInSceneStart()
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.alpha = 1f;
        blackScreen_CanvasGroup.DOFade(0, _blackFadeTime).onComplete = () => blackScreen_Panel.SetActive(false);
    }

    public void FadeOutScene(string nextScene)
    {
        blackScreen_CanvasGroup.alpha = 0f;
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).OnComplete(() => {
            SceneManager.LoadScene(nextScene);
        }).SetUpdate(true);
    }

    #endregion

    #region Fades with panels
    public void FadePanel(GameObject panel, bool estado)
    {
        blackScreen_Panel.SetActive(true);
        blackScreen_CanvasGroup.DOFade(1, _blackFadeTime).SetUpdate(true).onComplete = () => {
            panel.SetActive(estado);
            FadeInSceneStart();
        };
    }
    #endregion

    #region GET

    public bool GetBlackScreenOn()
    {
        return blackScreen_CanvasGroup.alpha == 1;
    }

    public bool GetBlackScreenOff()
    {
        return blackScreen_CanvasGroup.alpha == 0;
    }

    #endregion
}
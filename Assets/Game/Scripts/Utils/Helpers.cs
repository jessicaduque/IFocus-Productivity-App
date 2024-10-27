using UnityEngine;
using DG.Tweening;

public static class Helpers
{
    public static float blackFadeTime = 0.6f;
    public static float panelFadeTime = 0.4f;
    public static Camera cam => Camera.main;
    public static int screenWidth => Screen.width;
    public static int screenHeight => Screen.height;

    public static void FadeInPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.GetComponent<CanvasGroup>().DOFade(1, panelFadeTime).SetUpdate(true);
    }

    public static void FadeOutPanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().DOFade(0, panelFadeTime).OnComplete(() => panel.SetActive(false)).SetUpdate(true);
    }

    public static void FadeCrossPanel(GameObject offPanel, GameObject onPanel)
    {
        offPanel.GetComponent<CanvasGroup>().DOFade(0, panelFadeTime).OnComplete(() => {
            offPanel.SetActive(false);
            onPanel.SetActive(true);
            onPanel.GetComponent<CanvasGroup>().DOFade(1, panelFadeTime);
        });
    }
}
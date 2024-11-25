using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimizedTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _thisText;
    [SerializeField] private Image _fillImage;
    
    private MaximizedTimer _maximizedTimer = MaximizedTimer.I;
    private float _totalSeconds;
    private void OnEnable()
    {
        SetTotalSeconds();
        _maximizedTimer.endTimerAction += EndTimer;
    }

    private void OnDisable()
    {
        _maximizedTimer.endTimerAction -= EndTimer;
    }

    private void EndTimer()
    {
        _thisText.text = "";
        gameObject.SetActive(false);
    }

    private void SetTotalSeconds()
    {
        _totalSeconds = _maximizedTimer.GetTimerTotalSeconds();
    }
    
    void Update()
    {
        float secondsLeft = _maximizedTimer.GetTimerSecondsLeft();
        int hours = (int) (secondsLeft / 3600);
        int minutes = (int) ((secondsLeft - hours * 3600) / 60);
        _thisText.text = $"{hours:0}:{minutes:00}";
        _fillImage.fillAmount = Mathf.InverseLerp(0, _totalSeconds, secondsLeft);
    }
}

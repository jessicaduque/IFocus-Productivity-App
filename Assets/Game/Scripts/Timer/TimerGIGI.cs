using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TimerGIGI : MonoBehaviour , IPointerClickHandler
{

    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] public float durationSeconds = 10;
    private float _remainingDuration;
    private bool _isPaused;

    public void OnPointerClick(PointerEventData eventData)
    {
        _isPaused = !_isPaused;
    }

    void Start()
    {
        BeginTimer(durationSeconds);
    }

    private void BeginTimer (float second)
    {
        _remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        
        while (_remainingDuration >= 0) 
        {
            if (!_isPaused) 
            {
                uiText.text = $"{_remainingDuration / 60:00}:{_remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, durationSeconds, _remainingDuration);
                _remainingDuration -= Time.deltaTime;
            }
            yield return null;
            
        }
        TimerEnd();
    }

    private void TimerEnd()
    {
        Debug.Log("ACABOU");
    }

}

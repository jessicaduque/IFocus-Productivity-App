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
    private bool _isPaused = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        _isPaused = !_isPaused;
    }

    public void Pause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            //colocar a visibilidade da Runing
            //tirar a vizibilidade do Paused
        }
        else 
        { 
            _isPaused = true;
            //tirar a visibilidade da Runing
            //colocar a vizibilidade do Paused
        }
    }

    void Start()
    {
        BeginTimer(durationSeconds);
        //colocar a visibilidade da Runing
    }

    private void BeginTimer (float second)
    {
        _remainingDuration = second;
        //pergunta para a Jess se posso ter duas corrotinas ou se nesse caso é melhor usar o Update (para o botão de pausar e run)
        //ou, gambiarra, posso usar um contador ou algo que me indique que é a primeira vez que ele clicou e que é para iniciar o BeginTimer
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

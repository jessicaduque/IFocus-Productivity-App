using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TimerGIGI : MonoBehaviour , IPointerClickHandler
{

    [SerializeField] private Image uiFill;
    [SerializeField] private TMP_Text uiText;

    public float duration;
    private float remainingDuration;
    private bool Pause;

    public void OnPointerClick(PointerEventData eventData)
    {
        Pause = !Pause;
    }

    void Start()
    {
        Beging(duration);
    }

    private void Beging (float second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        
        while (remainingDuration >= 0) 
        {
            if (!Pause) 
            {
                uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);
                remainingDuration -= Time.deltaTime;
            }
            yield return null;
            
        }
        OnEnd();
    }

    private void OnEnd()
    {
        Debug.Log("ACABOU");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

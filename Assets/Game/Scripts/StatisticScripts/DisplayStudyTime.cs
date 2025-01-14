using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;

public class DisplayStudyTime : MonoBehaviour
{
    [SerializeField] private StudyTopic studyTopic; // Referência ao script StudyTopic
    [SerializeField] private TMP_Text timeText; // Referência ao componente TMP_Text para exibir o tempo

    private void Update()
    {
        if (studyTopic != null && timeText != null)
        {
            float totalSeconds = studyTopic.GetTimeStudiedTotalSeconds();

            // Converte os segundos para formato de horas, minutos e segundos
            int hours = Mathf.FloorToInt(totalSeconds / 3600);
            int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
            int seconds = Mathf.FloorToInt(totalSeconds % 60);

            // Atualiza o texto na tela
            timeText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
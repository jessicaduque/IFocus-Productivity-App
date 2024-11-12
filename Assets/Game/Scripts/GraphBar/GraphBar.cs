using UnityEngine;
using UnityEngine.UI;

public class ProportionalBarChart : MonoBehaviour
{
    public GameObject barPrefab;
    public Transform chartContainer; 
    public Text[] labelTexts; 
    public Text[] hourTexts; 
    public Text[] tagLabels;

    private int maxBars = 7;
    private float maxHeight = 300f;

    // Dados das tarefas - podem ser substituídos por valores dinâmicos
    public string[] taskNames = { "Tarefa 1", "Tarefa 2", "Tarefa 3", "Tarefa 4", "Tarefa 5", "Tarefa 6", "Tarefa 7" };
    public int[] hours = { 20, 5, 7, 4, 6, 3, 15 };

    void Start()
    {
        CreateBarChart();
    }

    void CreateBarChart()
    {
        int totalHours = 0;
        foreach (int hour in hours)
        {
            totalHours += hour;
        }

        Debug.Log("Total de horas na semana: " + totalHours);

        int barCount = Mathf.Min(maxBars, hours.Length);

        SetTagLabels(totalHours);

        for (int i = 0; i < barCount; i++)
        {
            GameObject bar = Instantiate(barPrefab, chartContainer);
            RectTransform barRect = bar.GetComponent<RectTransform>();
            barRect.anchoredPosition = new Vector2(i * 100, 0); 

            float normalizedHeight = (float)hours[i] / totalHours;
            float barHeight = normalizedHeight * maxHeight;
            barRect.sizeDelta = new Vector2(barRect.sizeDelta.x, barHeight);

            Debug.Log($"Tarefa: {taskNames[i]}, Horas: {hours[i]}, Altura da Barra: {barHeight}");

            if (labelTexts != null && i < labelTexts.Length)
                labelTexts[i].text = taskNames[i];

            if (hourTexts != null && i < hourTexts.Length)
                hourTexts[i].text = hours[i] + "h";
        }
    }

    void SetTagLabels(int totalHours)
    {
        int numTags = tagLabels.Length;
        float increment = (float)totalHours / (numTags - 1);

        for (int i = 0; i < numTags; i++)
        {
            int tagValue = Mathf.RoundToInt(i * increment);
            tagLabels[i].text = tagValue + "h";
            Debug.Log("Etiqueta lateral " + i + ": " + tagValue + "h");
        }
    }
}

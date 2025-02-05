using System;
using UnityEngine;
using UnityEngine.UI;

public class UITimeOfDayFilterController : MonoBehaviour
{
    [SerializeField] private Image morningImage;
    [SerializeField] private Image afternoonImage;
    [SerializeField] private Image nightImage;

    private Image _currentActiveImage;
    private int _lastCheckedHour;
    private int _lastCheckedMinute;

    private void Awake()
    {
        UpdateTimeOfDayImage();
    }

    private void Update()
    {
        int currentHour = DateTime.Now.Hour;
        int currentMinute = DateTime.Now.Minute;

        // Se o tempo ainda está dentro do mesmo período, não precisa mudar nada
        if (currentHour == _lastCheckedHour && currentMinute == _lastCheckedMinute)
            return;

        // Atualiza os registros de hora e minuto
        _lastCheckedHour = currentHour;
        _lastCheckedMinute = currentMinute;

        UpdateTimeOfDayImage();
    }

    private void UpdateTimeOfDayImage()
    {
        int currentHour = DateTime.Now.Hour;

        Image newActiveImage = null; // Começa assumindo que nenhuma imagem deve estar ativa

        if (currentHour >= 5 && currentHour <= 10) // Manhã
        {
            newActiveImage = morningImage;
        }
        else if (currentHour >= 17 && currentHour <= 19) // Tarde
        {
            newActiveImage = afternoonImage;
        }
        else if (currentHour >= 20 || currentHour <= 4) // Noite
        {
            newActiveImage = nightImage;
        }
        
        SetActiveImage(newActiveImage);
    }

    private void SetActiveImage(Image newActiveImage)
    {
        // Se já estamos na imagem correta, não precisa mudar nada
        if (_currentActiveImage == newActiveImage)
            return;

        // Desativa todas as imagens
        morningImage.gameObject.SetActive(false);
        afternoonImage.gameObject.SetActive(false);
        nightImage.gameObject.SetActive(false);

        // Ativa apenas a imagem correta, se houver uma
        if (newActiveImage != null)
        {
            newActiveImage.gameObject.SetActive(true);
        }

        // Atualiza a referência da imagem ativa
        _currentActiveImage = newActiveImage;
    }
}

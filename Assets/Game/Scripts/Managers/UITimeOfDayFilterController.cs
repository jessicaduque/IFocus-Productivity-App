using System;
using UnityEngine;
using UnityEngine.UI;

public class UITimeOfDayFilterController : MonoBehaviour
{
    [SerializeField] private Image morningImage;
    [SerializeField] private Image afternoonImage;
    [SerializeField] private Image nightImage;

    private void Awake()
    {
        UpdateTimeOfDayImage();
    }

    private void Update()
    {
        UpdateTimeOfDayImage();
    }

    private void UpdateTimeOfDayImage()
    {
        int currentHour = DateTime.Now.Hour;

        // Manhã: 5am - 11am
        if (currentHour >= 5 && currentHour <= 11)
        {
            SetActiveImage(morningImage);
        }
        // Tarde: 12pm - 6pm
        else if (currentHour >= 12 && currentHour <= 18)
        {
            SetActiveImage(afternoonImage);
        }
        // Noite: 7pm - 4am
        else
        {
            SetActiveImage(nightImage);
        }
    }

    private void SetActiveImage(Image activeImage)
    {
        // Desativa todas as imagens
        morningImage.gameObject.SetActive(false);
        afternoonImage.gameObject.SetActive(false);
        nightImage.gameObject.SetActive(false);

        // Ativa apenas a imagem correta
        activeImage.gameObject.SetActive(true);
    }
}
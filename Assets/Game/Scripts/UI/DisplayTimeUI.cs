using System;
using TMPro;
using UnityEngine;

public class DisplayTimeUI : MonoBehaviour
{
    private TextMeshProUGUI _timeText;

    private void Awake()
    {
        _timeText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _timeText.text = DateTime.Now.ToString("HH:mm");
    }
}


using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DisplayDateUI : MonoBehaviour
{
    private TextMeshProUGUI _dateText;

    public Dictionary <int, string> months;

    private void Awake()
    {

        months = new Dictionary<int, string>() {
            { 1, "Jan" },
            { 2, "Feb" },
            { 3, "Mar" },
            { 4, "Apr" },
            { 5, "May" },
            { 6, "Jun" },
            { 7, "Jul" },
            { 8, "Aug" },
            { 9, "Sep" },
            { 10, "Oct" },
            { 11, "Nov" },
            { 12, "Dec" }

        }; 
        
        _dateText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        int num_month = System.DateTime.Now.Month;
        int num_day = System.DateTime.Now.Day;

        string text_month = months[num_month];
        _dateText.text = $"{text_month} {num_day}";
    }
}

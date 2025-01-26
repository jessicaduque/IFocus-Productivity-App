using TMPro;
using UnityEngine;

public class DisplayDateUI : MonoBehaviour
{
    private TextMeshProUGUI _dateText;

    private void Awake()
    {
        _dateText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _dateText.text = System.DateTime.Now.ToString("dd/MM");
    }
}

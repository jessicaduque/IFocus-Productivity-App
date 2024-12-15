using System;
using UnityEngine;
using TMPro;

public abstract class TextMatchInputField : MonoBehaviour
{
    protected TextMeshProUGUI thisText;
    [SerializeField] protected TMP_InputField thisInputField;
    
    private void Awake()
    {
        thisText = GetComponent<TextMeshProUGUI>();
        thisInputField.onValueChanged.AddListener(ControlInput);
    }
    
    protected virtual void ControlInput(string text)
    {
    }

    protected virtual void MatchTexts(string text)
    {
    }
}

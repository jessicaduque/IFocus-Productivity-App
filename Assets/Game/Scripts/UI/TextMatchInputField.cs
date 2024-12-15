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

    private void Start()
    {
        thisText.text = thisInputField.text;
    }

    protected virtual void ControlInput(string text)
    {
    }

    protected virtual void MatchTexts(string text)
    {
    }
}

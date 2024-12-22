using System;
using TMPro;
using UnityEngine;

public class InputFieldCharacterCounter : MonoBehaviour
{
    TextMeshProUGUI thisText;
    [SerializeField] TMP_InputField thisInputField;
    [SerializeField] private int maxAmountCharacters;
    private void Awake()
    {
        thisText = GetComponent<TextMeshProUGUI>();
        thisInputField.characterLimit = maxAmountCharacters;
        thisInputField.onValueChanged.AddListener(delegate
        {
            int amountCharacter = thisInputField.text.Length;
            thisText.text =  amountCharacter + "/" + maxAmountCharacters; 
        });
    }

    private void OnEnable()
    {
        thisInputField.text = "";
        thisText.text = "0/" + maxAmountCharacters;
    }
}

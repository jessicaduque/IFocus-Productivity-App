using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldCharacterCounter : MonoBehaviour
{
    TextMeshProUGUI thisText;
    [SerializeField] TMP_InputField thisInputField;
    [SerializeField] private int maxAmountCharacters;
    [SerializeField] private Button associatedButton; 
    private void Awake()
    {
        thisText = GetComponent<TextMeshProUGUI>();
        thisInputField.characterLimit = maxAmountCharacters;
        
        thisInputField.onValueChanged.AddListener(delegate
        {
            int amountCharacter = thisInputField.text.Length;
            thisText.text =  amountCharacter + "/" + maxAmountCharacters;
            if (associatedButton == null) return;
            associatedButton.interactable = amountCharacter != 0;
        });
    }

    private void Start()
    {
        associatedButton.interactable = false;
    }

    private void OnEnable()
    {
        thisInputField.text = "";
        thisText.text = "0/" + maxAmountCharacters;
    }
}

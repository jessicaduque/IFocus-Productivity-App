using UnityEngine;
public class TextMatchInputFieldTimer : TextMatchInputField
{
    [SerializeField] private bool isHours;
    protected override void ControlInput(string text)
    {
        if (isHours)
        {
            if (thisInputField.text.Length == 1 && thisInputField.text[0] == '0')
                thisInputField.text = "";
            MatchTexts(thisInputField.text);
            return;
        }
        
        
        if (thisInputField.text.Length == 1)
        {
            char character1 = thisInputField.text[0];
            if (character1 == '0')
            {
                thisInputField.text = "";
            }
        }
        else if (thisInputField.text.Length == 2)
        {
            char character1 = thisInputField.text[0];
            char character2 = thisInputField.text[1];
            
            if (int.Parse(char.ToString(character1)) > 5)
            {
                if (character1 != '6' || character2 != '0')
                {
                    thisInputField.text = character1.ToString();
                }
            }
        }
        
        MatchTexts(thisInputField.text);
    }

    protected override void MatchTexts(string text)
    {
        int time = (text != "" ? int.Parse(text) : 0);
        thisText.text = (isHours ? $"{time:0}" : $"{time:00}");
    }
}
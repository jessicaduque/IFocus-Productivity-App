public class TextMatchInputFieldStudyTopic : TextMatchInputField
{
    private void Start()
    {
        thisText.text = thisInputField.text;
    }
    
    protected override void ControlInput(string text)
    {
        MatchTexts(thisInputField.text);
    }

    protected override void MatchTexts(string text)
    {
        thisText.text = text;
    }
}

public class TextMatchInputFieldStudyTopic : TextMatchInputField
{
    protected override void ControlInput(string text)
    {
        MatchTexts(thisInputField.text);
    }

    protected override void MatchTexts(string text)
    {
        thisText.text = text;
    }
}

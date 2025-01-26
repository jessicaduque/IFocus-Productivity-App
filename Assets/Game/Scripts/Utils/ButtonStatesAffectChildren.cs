using UnityEngine;
using UnityEngine.UI;

public class ButtonStatesAffectChildren : Button
{
    private Image[] _childrenImages;
    protected Graphic[] _graphics;

    protected override void OnValidate()
    {
        base.OnValidate();

        _childrenImages = GetComponentsInChildren<Image>();
        _graphics = GetComponentsInChildren<Graphic>();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (_childrenImages.Length != 1)
        {
            if (state == SelectionState.Disabled)
            {
                foreach (Image im in _childrenImages)
                {
                    Color colorTest = im.color;
                    colorTest.a = 0.5f;
                    im.color = colorTest;
                }
            }
            else if (state == SelectionState.Normal)
            {
                foreach (Image im in _childrenImages)
                {
                    Color colorTest = im.color;
                    colorTest.a = 1f;
                    im.color = colorTest;
                }
            }
        }
        
        var targetColor =
            state == SelectionState.Disabled ? colors.disabledColor :
            state == SelectionState.Highlighted ? colors.highlightedColor :
            state == SelectionState.Normal ? colors.normalColor :
            state == SelectionState.Pressed ? colors.pressedColor :
            state == SelectionState.Selected ? colors.selectedColor : Color.white;
        
        for (int i = 1; i < _graphics.Length; i++)
        {
            _graphics[i].CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
    }
}
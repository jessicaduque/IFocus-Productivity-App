using UnityEngine;
using UnityEngine.UI;

public class ButtonStatesAffectChildren : Button
{
    protected Graphic[] _graphics;

    protected override void Awake()
    {
        base.Awake();
        
        _graphics = GetComponentsInChildren<Graphic>();
    }

protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        var targetColor =
            state == SelectionState.Disabled ? colors.disabledColor :
            state == SelectionState.Highlighted ? colors.highlightedColor :
            state == SelectionState.Normal ? colors.normalColor :
            state == SelectionState.Pressed ? colors.pressedColor :
            state == SelectionState.Selected ? colors.selectedColor : Color.white;

        if (_graphics != null)
        {
            for (int i = 1; i < _graphics.Length; i++)
            {
                _graphics[i].CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }
    }
}
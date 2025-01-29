using UnityEngine;
using UnityEngine.UI;

public class ButtonStatesAffectChildren : Button
{
    protected Graphic[] _graphics;
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
    
        _graphics = GetComponentsInChildren<Graphic>();
    }
#endif
    
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
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
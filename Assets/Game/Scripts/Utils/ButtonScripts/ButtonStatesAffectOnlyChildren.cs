using UnityEngine;

public class ButtonStatesAffectOnlyChildren : ButtonStatesAffectChildren
{
    private Color _transparentColor = new Color(255, 255, 255, 0);
    
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        _graphics[0].color = _transparentColor;
    }
}

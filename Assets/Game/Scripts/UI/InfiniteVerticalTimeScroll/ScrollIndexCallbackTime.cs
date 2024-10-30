using TMPro;
using UnityEngine;

public class ScrollIndexCallbackTime : MonoBehaviour
{
    public TextMeshProUGUI text;
    void ScrollCellIndex(int idx)
    {
        if (idx < 0) idx = (((int) (Mathf.Abs(idx) / 60) + 1) * 60) - Mathf.Abs(idx);
        else idx %= 60;

        if (idx == 60) idx = 0;

        string numberText;
        if (idx < 10) numberText = "0" + idx.ToString();
        else numberText = idx.ToString();

        if (text != null)
        {
            text.text = numberText;
        }
    }
}

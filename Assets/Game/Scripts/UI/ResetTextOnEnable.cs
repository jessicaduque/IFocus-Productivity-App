using TMPro;
using UnityEngine;

public class ResetTextOnEnable : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI this_text;

    private void OnEnable()
    {
        this_text.text = "";
    }
}

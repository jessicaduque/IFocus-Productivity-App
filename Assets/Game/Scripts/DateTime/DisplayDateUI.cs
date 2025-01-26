using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public TMP_Text date;

    private void OnEnable()
    {
        date.text = System.DateTime.Now.ToString("dd/MM");

    }
}

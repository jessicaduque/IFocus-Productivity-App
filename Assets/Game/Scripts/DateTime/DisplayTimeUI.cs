using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTimeUI : MonoBehaviour
{
    public TMP_Text time;

    private void Update()
    {
        time.text = System.DateTime.Now.ToString("HH:mm");
    }
}


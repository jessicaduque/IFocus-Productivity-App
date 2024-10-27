using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class TodoListObject : MonoBehaviour
{
    public string objName;
    public string topic;
    public bool isChecked;

    [SerializeField] Image im_line;
    [SerializeField] TextMeshProUGUI t_item;

    public void SetObjectInfo(string name, string type, bool isChecked)
    {
        this.objName = name;
        this.topic = type;
        this.isChecked = isChecked;

        t_item.text = objName;
        im_line.gameObject.SetActive(true);
    }
}

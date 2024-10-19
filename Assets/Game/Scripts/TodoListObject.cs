using UnityEngine;
using TMPro; // Biblioteca de texto 

public class TodoListObject : MonoBehaviour
{
    public string objName;
    public string type;
    public int index;

    TextMeshProUGUI t_item;

    private void Awake()
    {
        t_item = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetObjectInfo(string name, string type, int index)
    {
        this.objName = name;
        this.type = type;
        this.index = index;

        t_item.text = objName;
    }
}

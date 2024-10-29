using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Utils.Singleton;

public class TodoListManager : Singleton<TodoListManager>
{
    [SerializeField] Transform _content;
    [SerializeField] GameObject _addPanel;
    [SerializeField] Button b_create;
    [SerializeField] GameObject _todoListItemPrefab;
    [SerializeField] TMP_InputField[] _addInputFields;
    
    List<TodoListObject> _todoListObjects = new List<TodoListObject>();
    int _characterLimit = 30;
    int _amountListObjects = 0;
    
    string _filePath => Application.persistentDataPath + "/todolist.txt";

    private void Awake()
    {
        _addInputFields = _addPanel.GetComponentsInChildren<TMP_InputField>();
        b_create.onClick.AddListener(delegate { CreateTodoListItem(_addInputFields[0].text, _addInputFields[1].text); } );
        InitInputFields();
    }

    private void Start()
    {
        LoadJSONData();
    }
    
    // Creates a new item to add to the to-do list
    private void CreateTodoListItem(string name, string topic, bool isChecked=false, bool isLoading=false)
    {
        if (!isLoading && !AreInputFieldsFilledIn())
        {
            ShowInputFieldError();
            return;
        }

        GameObject item = Instantiate(_todoListItemPrefab, _content);
        TodoListObject itemObject = item.GetComponent<TodoListObject>();
        itemObject.SetObjectInfo(name, topic, isChecked);
        _todoListObjects.Add(itemObject);
        TodoListObject temp = itemObject;
        Toggle itemToggle = itemObject.GetComponent<Toggle>();
        itemToggle.isOn = isChecked;
        itemToggle.onValueChanged.AddListener(delegate { CheckItem(temp); });
        
        _amountListObjects++;
        if (!isLoading)
        {
            SaveJSON();
            SwitchMode(0);
        }
        else
        {
            itemObject.ChangeLineState();
        }
    }

    #region Change Item Status
    // Checks an item on the list. With that, it is deleted from the list.
    private void CheckItem(TodoListObject item)
    {
        if (item.isChecked)
        {
            UncheckItem(item);
            return;
        }

        item.ChangeLineState(true);
        SaveJSON();
    }

    private void UncheckItem(TodoListObject item)
    {
        item.ChangeLineState(false);
        SaveJSON();
    }

    public void DeleteItem(TodoListObject item)
    {
        _todoListObjects.Remove(item);
        _amountListObjects--;
        SaveJSON();
        Destroy(item.gameObject);
    }
    #endregion
    #region Input Fields

    // Initiates initial configurations for the input fields
    private void InitInputFields()
    {
        foreach (TMP_InputField inputField in _addInputFields)
        {
            inputField.characterLimit = _characterLimit; // Sets the character limit for each input field
        }
    }

    // Checks if all the input fields have some form of text in them
    private bool AreInputFieldsFilledIn()
    {
        foreach (TMP_InputField inputField in _addInputFields)
        {
            if (inputField.text == "") return false;
        }

        return true;
    }

    // Clears the text present on the input fields
    private void ClearInputFields()
    {
        foreach (TMP_InputField inputField in _addInputFields)
        {
            inputField.text = "";
        }
    }
    // Shows a visual error to the player if any of the input fields hasn't been filled in
    private void ShowInputFieldError()
    {
        // Codigo para mostrar erro criacao
    }

    #endregion
    // Switches the panel mode (between adding an item or just showing the to-do list.
    public void SwitchMode(int mode)
    {
        switch (mode)
        {
            // To-do list normal show mode
            case 0:
                _addPanel.SetActive(false);
                ClearInputFields();
                break;
            // To-do list adding item mode
            case 1:
                _addPanel.SetActive(true);
                break;
        }
    }

    #region JSON Data

    // Saves the to-do list data into a JSON file
    private void SaveJSON()
    {
        string contents = "";
        
        for (int todoListObjectIndex = 0; todoListObjectIndex < _amountListObjects; todoListObjectIndex++)
        {
            TodoListItem temp = new TodoListItem(_todoListObjects[todoListObjectIndex].objName, _todoListObjects[todoListObjectIndex].topic, _todoListObjects[todoListObjectIndex].isChecked);
            contents += JsonUtility.ToJson(temp) + "\n";
        }

        File.WriteAllText(_filePath, contents);
    }
    // Loads any data that has already been previously saved to the to-do list 
    private void LoadJSONData()
    {
        if (File.Exists(_filePath))  
        {
            string contents = File.ReadAllText(_filePath);

            string[] splitContents = contents.Split('\n');

            _amountListObjects = 0; // Confirmar que o index comece como 0 antes de fazer load de dados
            foreach (string content in splitContents)
            {
                if(content.Trim() != "")
                {
                    TodoListItem temp = JsonUtility.FromJson<TodoListItem>(content);
                    CreateTodoListItem(temp.objName, temp.topic, temp.isChecked, true);
                }
            }
        }
    }

    #endregion

    // Public class inside class to help with serialization
    public class TodoListItem
    {
        public string objName;
        public string topic;
        public bool isChecked;

        public TodoListItem(string name, string topic, bool isChecked)
        {
            this.objName = name;
            this.topic = topic;
            this.isChecked = isChecked;
        }
    }
}

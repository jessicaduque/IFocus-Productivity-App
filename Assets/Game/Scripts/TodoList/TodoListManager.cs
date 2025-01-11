using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils.Singleton;

public class TodoListManager : Singleton<TodoListManager>
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _addPanel;
    [SerializeField] private Button b_addPanelOpen;
    [SerializeField] private Button b_addPanelClose;
    [SerializeField] private Button b_createItem;
    [SerializeField] private Button b_close;
    [SerializeField] private TextMeshProUGUI _amountItemsText;
    [SerializeField] private GameObject _todoListItemPrefab;
    [SerializeField] private TMP_InputField _addInputField;
    [SerializeField] private TMP_Dropdown _topicDropdown;
    
    private List<TodoListObject> _todoListObjects = new List<TodoListObject>();
    private int _characterLimitName = 30;
    private int _maxAmountListObjects = 40;
    private int _amountListObjects = 0;

    private JSONManager _jsonManager => JSONManager.I;
    private StudyTopicsManager _studyTopicsManager => StudyTopicsManager.I; 
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;

    protected override void Awake()
    {
        base.Awake();
        b_createItem.onClick.AddListener(delegate { CreateTodoListItem(_addInputField.text, _topicDropdown.captionText.text); } );
        b_addPanelOpen.onClick.AddListener(delegate { SwitchMode(1); } );
        b_addPanelClose.onClick.AddListener(delegate { SwitchMode(0); } );
        b_close.onClick.AddListener(delegate { _uiPanelsManager.ControlTodoListPanel(false); } );
        InitInputField();
    }

    private void Start()
    {
        LoadJSONData();
    }
    
    // Creates a new item to add to the to-do list
    public void CreateTodoListItem(string name, string topic, bool isChecked=false, bool isLoading=false)
    {
        if (!isLoading && !IsInputFieldFilled())
        {
            ShowInputFieldError();
            return;
        }

        if (_amountListObjects < _maxAmountListObjects)
        {
            GameObject item = Instantiate(_todoListItemPrefab, _content);
            TodoListObject itemObject = item.GetComponent<TodoListObject>();
            itemObject.SetObjectInfo(name, topic, isChecked);
            _todoListObjects.Add(itemObject);
            TodoListObject temp = itemObject;
            Toggle itemToggle = itemObject.GetComponent<Toggle>();
            itemToggle.isOn = isChecked;
            itemToggle.onValueChanged.AddListener(delegate { CheckItem(temp); });
        
            _amountListObjects++;
            
            if(_amountListObjects == 40)
            {
                b_createItem.interactable = false;
            }
            
            _amountItemsText.text = _amountListObjects.ToString() + "/" + _maxAmountListObjects.ToString();
                
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

    public void EditToDoItemByStudyTopic(string originalTopicName, string newTopicName)
    {
        foreach (TodoListObject todoObject in _todoListObjects)
        {
            if (todoObject.topic == originalTopicName)
            {
                todoObject.SetType(newTopicName);
                SaveJSON();
            }
        }
    }
    public void DeleteToDoItemByStudyTopic(string topicName)
    {
        for (int todoObjectIndex = 0; todoObjectIndex < _amountListObjects; todoObjectIndex++)
        {
            if (_todoListObjects[todoObjectIndex].topic == topicName)
            {
                DeleteItem(_todoListObjects[todoObjectIndex]);
                todoObjectIndex--;
            }
        }
    }
    public void DeleteItem(TodoListObject item)
    {
        _todoListObjects.Remove(item);
        if(_amountListObjects == _maxAmountListObjects)
        {
            b_createItem.interactable = true;
        }
        _amountListObjects--;
        _amountItemsText.text = _amountListObjects.ToString() + "/" + _maxAmountListObjects.ToString();
        SaveJSON();
        Destroy(item.gameObject);
    }
    #endregion
    #region User Input Fields UI

    // Initiates initial configurations for the input field
    private void InitInputField()
    {
        _addInputField.characterLimit = _characterLimitName; // Sets the character limit input field
    }
    // Initiates initial configurations for the dropdown
    public void ConfigureDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        
        foreach (StudyTopic topic in _studyTopicsManager.GetStudyTopics())
        {
            options.Add(new TMP_Dropdown.OptionData(topic.GetObjName()));
        }

        _topicDropdown.options = options;
        _topicDropdown.value = 0;
    }

    // Checks if all the input fields have some form of text in them
    private bool IsInputFieldFilled()
    {
        return _addInputField.text != "";
    }

    // Clears the text present on the input fields
    private void ClearInputField()
    {
        _addInputField.text = "";
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
                break;
            // To-do list adding item mode
            case 1:
                ConfigureDropdown();
                ClearInputField();
                _addPanel.SetActive(true);
                break;
        }
    }

    #region JSON Data

    // Saves the to-do list data into a JSON file
    private void SaveJSON()
    {
        _jsonManager.SaveTodoList(_todoListObjects, _amountListObjects);
    }
    // Loads any data that has already been previously saved to the to-do list 
    private void LoadJSONData()
    {
        _jsonManager.LoadTodoList();
    }

    #endregion
}

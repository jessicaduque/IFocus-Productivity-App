using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class StudyTopicsManager : Singleton<StudyTopicsManager>
{
    [SerializeField] Transform _content;
    [SerializeField] TMP_InputField _addInputField;
    [SerializeField] private TextMeshProUGUI _amountItemsText;
    [SerializeField] Button b_create;
    [SerializeField] GameObject _studyTopicPrefab;
    
    private List<string> _listItems = new List<string>();
    private int _characterLimitName = 30;
    private int _maxAmountTopicsObjects = 40;
    private int _amountTopicsObjects = 0;
    
    private JSONManager _jsonManager => JSONManager.I;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    
    protected override void Awake()
    {
        base.Awake();
        b_create.onClick.AddListener(delegate { CreateStudyTopicItem(_addInputField.text); } );
        InitInputField();
    }

    private void Start()
    {
        LoadJSONData();
    }
    
    // Creates a new item to add to the study topics
    public void CreateStudyTopicItem(string name, bool isLoading=false, bool isDefault=false)
    {
        if (!isLoading && !IsInputFieldFilled())
        {
            ShowInputFieldError();
            return;
        }

        foreach (string nameItem in _listItems)
        {
            if (nameItem == name)
            {
                ShowNameAlreadyExistentError();
                return;
            }
        }

        if (_amountTopicsObjects < _maxAmountTopicsObjects)
        {
            _listItems.Add(name);
            GameObject item;
            if (isDefault)
            {
                item = _content.GetChild(0).gameObject;
            }
            else
            {
                item = Instantiate(_studyTopicPrefab, _content);
            }
            StudyTopic studyTopic = item.GetComponent<StudyTopic>();
            TMP_InputField inputField = studyTopic.GetInputField();
            studyTopic.SetObjectInfo(name, isDefault);
            studyTopic.GetEditButton().onClick.AddListener(inputField.ActivateInputField);
            inputField.onEndEdit.AddListener(delegate { EditStudyTopic(name); });
            studyTopic.GetDeleteButton().onClick.AddListener(delegate { _uiPanelsManager.ControlDeleteTopicWarningPanel(true); });
            
            _amountTopicsObjects++;
            
            if(_amountTopicsObjects == _maxAmountTopicsObjects)
            {
                b_create.interactable = false;
            }
            
            _amountItemsText.text = _amountTopicsObjects.ToString() + "/" + _maxAmountTopicsObjects.ToString();
                
            if (!isLoading)
            {
                SaveJSON(name);
                ClearInputField();
            }
        }
        else
        {
            // Código para sinalizar ao usuário que a lista já está cheia!
        }
        
    }

    private void EditStudyTopic(string name)
    {
        
    }
    
    #region User Input Fields UI

    // Initiates initial configurations for the input field
    private void InitInputField()
    {
        _addInputField.characterLimit = _characterLimitName; // Sets the character limit input field
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
    // Shows a visual error to the player if the topic name already exists
    private void ShowNameAlreadyExistentError()
    {
        // Codigo para mostrar erro nome
    }

    #endregion
    
    #region JSON Data

    // Saves a study topic data into a JSON file
    private void SaveJSON(string topicName)
    {
        _jsonManager.SaveStudyTopics(topicName, _amountTopicsObjects);
    }
    // Loads any data that has already been previously saved to the to-do list 
    private void LoadJSONData()
    {
        _jsonManager.LoadStudyTopics();
    }

    #endregion
}

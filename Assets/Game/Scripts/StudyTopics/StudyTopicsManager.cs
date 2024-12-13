using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StudyTopicsManager : Utils.Singleton.Singleton<StudyTopicsManager>
{
    [SerializeField] Transform _content;
    [SerializeField] TMP_InputField _addInputField;
    [SerializeField] private TextMeshProUGUI _amountItemsText;
    [SerializeField] Button b_create;
    [SerializeField] GameObject _studyTopicPrefab;
    
    private List<StudyTopic> _listItems = new List<StudyTopic>();
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
    public void CreateStudyTopicItem(string topicName, bool isLoading=false, bool isDefault=false)
    {
        if (!isLoading && !IsInputFieldFilled())
        {
            ShowInputFieldError();
            return;
        }

        if (TopicNameAlreadyExists(topicName))
        {
            ShowNameAlreadyExistentError();
            return;
        }

        if (_amountTopicsObjects < _maxAmountTopicsObjects)
        {
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
            _listItems.Add(studyTopic);
            TMP_InputField inputField = studyTopic.GetInputField();
            studyTopic.SetObjectInfo(topicName, isDefault);
            studyTopic.GetEditButton().onClick.AddListener(() =>
            {
                inputField.interactable = true;
                inputField.ActivateInputField();
            });
            inputField.onEndEdit.AddListener(delegate { CheckAndEditStudyTopic(topicName, inputField); });
            studyTopic.GetDeleteButton()?.onClick.AddListener(delegate { _uiPanelsManager.ControlDeleteTopicWarningPanel(true, topicName); });
            
            _amountTopicsObjects++;
            
            if(_amountTopicsObjects == _maxAmountTopicsObjects)
            {
                b_create.interactable = false;
            }
            
            _amountItemsText.text = _amountTopicsObjects.ToString() + "/" + _maxAmountTopicsObjects.ToString();
                
            if (!isLoading)
            {
                SaveJSON();
                ClearInputField();
            }
        }
        else
        {
            // Código para sinalizar ao usuário que a lista já está cheia!
        }
        
    }

    private void CheckAndEditStudyTopic(string originalName, TMP_InputField inputField)
    {
        string newName = inputField.text;
        if (TopicNameAlreadyExists(newName) ||newName.Length == 0 || newName.Length > _characterLimitName)
        {
            inputField.text = originalName;
            inputField.interactable = false;
            return;
        }

        foreach (StudyTopic topic in _listItems)
        {
            if (topic.objName == originalName)
            {
                _jsonManager.ChangeStudyTopicName(topic.objName, newName);
                topic.objName = newName;
            }
        }
        
        inputField.interactable = false;
    }


    public void DeleteTopic(string topicName)
    {
        foreach (StudyTopic topic in _listItems)
        {
            if (topic.objName == topicName)
            {
                _jsonManager.DeleteStudytopic(topicName);
                _listItems.Remove(topic);
                Destroy(topic.gameObject);
                return;
            }
        }
    }
    
    private bool TopicNameAlreadyExists(string newName)
    {
        foreach (StudyTopic topic in _listItems)
        {
            if (topic.objName == newName)
            {
                ShowNameAlreadyExistentError();
                return true;
            }
        }

        return false;
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
    private void SaveJSON()
    {
        _jsonManager.SaveStudyTopics(_listItems, _amountTopicsObjects);
    }
    // Loads any data that has already been previously saved to the to-do list 
    private void LoadJSONData()
    {
        _jsonManager.LoadStudyTopics();
    }

    #endregion
}

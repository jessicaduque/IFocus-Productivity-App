using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class StudyTopicsManager : Singleton<StudyTopicsManager>
{
    [SerializeField] Transform _content;
    [SerializeField] TMP_InputField _addInputField;
    [SerializeField] Button b_create;
    [SerializeField] GameObject _studyTopicPrefab;
    
    int _amountListObjects = 0;
    private List<string> _listItems = new List<string>();
    private JSONManager _jsonManager => JSONManager.I;
    #region JSON Data

    // Saves a study topic data into a JSON file
    private void SaveJSON(string topicName)
    {
        _jsonManager.SaveStudyTopics(topicName, _amountListObjects);
    }
    // Loads any data that has already been previously saved to the to-do list 
    private void LoadJSONData()
    {
        _jsonManager.LoadStudyTopics();
    }

    #endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class StudyTopicsManager : Singleton<StudyTopicsManager>
{
    [SerializeField] Transform _content;
    [SerializeField] Button b_create;
    [SerializeField] GameObject _todoListItemPrefab;
    [SerializeField] TMP_InputField _addInputField;
    
    int _amountListObjects = 0;
}

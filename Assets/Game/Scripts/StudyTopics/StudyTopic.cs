using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class StudyTopic : MonoBehaviour
{
    [SerializeField] TMP_InputField _thisInputField;
    [SerializeField] private Button b_delete;
    [SerializeField] private Button b_edit;

    private string _objName;
    private bool _isDefault;
    private float _timeStudiedTotalSeconds;

    private void Awake()
    {
        // PARA TESTES DE ESTATÍSTICAS
        _timeStudiedTotalSeconds = Random.Range(120f, 3000f);
    }

    public UnityAction<string> nameAlteredAction;
    public void SetObjectInfo(string name, bool isDefault, float timeStudiedTotalSeconds)
    {
        this._objName = name;
        this._isDefault = isDefault;
        _thisInputField.text = _objName;
        this._timeStudiedTotalSeconds = timeStudiedTotalSeconds;
    }

    
    #region Set

    public void SetObjName(string objName)
    {
        this._objName = objName;
        nameAlteredAction?.Invoke(objName);
    }

    public void SetIsDefault(bool isDefault)
    {
        this._isDefault = isDefault;
    }

    public void SetTimeStudiedTotalSeconds(float timeStudiedTotalSeconds)
    {
        _timeStudiedTotalSeconds = timeStudiedTotalSeconds;
    }
    
    #endregion
    #region Get

    public string GetObjName()
    {
        return _objName;
    }
    public bool GetIsDefault()
    {
        return _isDefault;
    }
    
    public float GetTimeStudiedTotalSeconds()
    {
        return _timeStudiedTotalSeconds;
    }
    
    public Button GetDeleteButton()
    {
        return b_delete;
    }
    public Button GetEditButton()
    {
        return b_edit;
    }
    public TMP_InputField GetInputField()
    {
        return _thisInputField;
    }
    #endregion
}

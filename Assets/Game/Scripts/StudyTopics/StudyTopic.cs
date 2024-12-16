using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StudyTopic : MonoBehaviour
{
    [SerializeField] TMP_InputField _thisInputField;
    [SerializeField] private Button b_delete;
    [SerializeField] private Button b_edit;

    private string objName;
    private bool isDefault;

    public UnityAction<string> nameAlteredAction;
    public void SetObjectInfo(string name, bool isDefault)
    {
        this.objName = name;
        this.isDefault = isDefault;
        _thisInputField.text = objName;
    }

    
    #region Set

    public void SetObjName(string objName)
    {
        this.objName = objName;
        nameAlteredAction?.Invoke(objName);
    }

    public void SetIsDefault(bool isDefault)
    {
        this.isDefault = isDefault;
    }
    
    #endregion
    #region Get

    public string GetObjName()
    {
        return objName;
    }
    public bool GetIsDefault()
    {
        return isDefault;
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudyTopic : MonoBehaviour
{
    [SerializeField] TMP_InputField _thisInputField;
    [SerializeField] private Button b_delete;
    [SerializeField] private Button b_edit;

    private string objName;
    [SerializeField] bool isDefault;    
    public void SetObjectInfo(string name, bool isDefault)
    {
        this.objName = name;
        this.isDefault = isDefault;
        _thisInputField.text = objName;
    }

    #region Get
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

using UnityEngine;
using TMPro;
using UnityEngine.UI; 
public class TodoListObject : MonoBehaviour
{
    public string objName;
    public string topic;
    public bool isChecked;

    [SerializeField] Image im_line;
    [SerializeField] TextMeshProUGUI t_item;
    [SerializeField] private Button b_delete;

    private StudyTopic _thisStudyTopic;
    private TodoListManager _todoListManager => TodoListManager.I;
    private StudyTopicsManager _studyTopicsManager => StudyTopicsManager.I; 
    private void Awake()
    {
        b_delete.onClick.AddListener(delegate { _todoListManager.DeleteItem(this); });
    }

    private void Start()
    {
        ChangeLineState();
    }

    public void ChangeLineState(bool activated)
    {
        isChecked = activated;
        im_line.enabled = activated;
        b_delete.gameObject.SetActive(activated);
    }

    public void ChangeLineState()
    {
        ChangeLineState(isChecked);
    }

    public void SetObjectInfo(string objName, string type, bool isChecked)
    {
        this.objName = objName;
        this.topic = type;
        this.isChecked = isChecked;
        this._thisStudyTopic = _studyTopicsManager.GetSpecificStudyTopic(type);
        
        t_item.text = objName;
        im_line.gameObject.SetActive(true);
    }

    public void SetType(string newTypeName)
    {
        this.topic = newTypeName;
    }
}

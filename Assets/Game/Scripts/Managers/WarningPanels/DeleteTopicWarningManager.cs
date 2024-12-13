using UnityEngine;
using UnityEngine.UI;

public class DeleteTopicWarningManager : MonoBehaviour
{
    [SerializeField] private Button b_cancel, b_accept;
    private string deleteTopicName;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    StudyTopicsManager _studyTopicsManager => StudyTopicsManager.I;
    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        b_cancel.onClick.AddListener(delegate { _uiPanelsManager.ControlDeleteTopicWarningPanel(false); });
        b_accept.onClick.AddListener(delegate { _studyTopicsManager.DeleteTopic(deleteTopicName); _uiPanelsManager.ControlDeleteTopicWarningPanel(false); });
    }

    public void SetDeleteTopicName(string deleteTopicName)
    {
        this.deleteTopicName = deleteTopicName;
    }

}

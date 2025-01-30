using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Game.Scripts.Audio;
using TMPro;

public class DeleteTopicWarningManager : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button acceptButton;
    Sequence sequenceOpen;
    [SerializeField] private Transform[] imagesForAnimationOn = new Transform[3];
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private string[] deleteTopicStrings = new string[2];
    private string _deleteTopicName;
    private float _panelTime => Helpers.panelFadeTime;
    private UIPanelsManager _uiPanelsManager => UIPanelsManager.I;
    private StudyTopicsManager _studyTopicsManager => StudyTopicsManager.I;
    private AudioManager _audioManager => AudioManager.I;
    private void Awake()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        _audioManager.PlaySfx("panelWarning");
        
        transform.localScale = Vector3.one;
        foreach (Transform panelTransform in imagesForAnimationOn)
        {
            panelTransform.localScale = Vector3.zero;
        }
        sequenceOpen = DOTween.Sequence();
        
        sequenceOpen.Join(imagesForAnimationOn[0].DOScale(1, _panelTime));
        sequenceOpen.Insert(_panelTime / 3, imagesForAnimationOn[1].DOScale(1, _panelTime));
        sequenceOpen.Insert(2 * _panelTime / 3, imagesForAnimationOn[2].DOScale(1, _panelTime));
    }

    private void SetupButtons()
    {
        cancelButton.onClick.AddListener(delegate { _uiPanelsManager.ControlDeleteTopicWarningPanel(false); });
        acceptButton.onClick.AddListener(delegate { _studyTopicsManager.DeleteTopic(_deleteTopicName); _uiPanelsManager.ControlDeleteTopicWarningPanel(false); });
    }

    public void SetDeleteTopicName(string deleteTopicName)
    {
        this._deleteTopicName = deleteTopicName;

        descriptionText.text = deleteTopicStrings[0] + deleteTopicName + deleteTopicStrings[1];
    }

    public void ClosePanel()
    {
        transform.DOScale(0, _panelTime).OnComplete(delegate
        {
            gameObject.SetActive(false);
        });
    }
}

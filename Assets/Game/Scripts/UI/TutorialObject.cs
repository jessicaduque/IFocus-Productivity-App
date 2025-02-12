using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private Button thisRelatedButton;
    [SerializeField] private GameObject thisRelatedGameBigBubbleText;
    private string _playerPrefsTutorialID = "IsTutorial_";
    private GameObject _thisGameObject;
    private float _panelTime => Helpers.panelFadeTime;
    private UIPanelsManager _uIPanelsManager => UIPanelsManager.I;
    private void Awake()
    {
        _playerPrefsTutorialID += gameObject.name;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(_playerPrefsTutorialID))
        {
            thisRelatedGameBigBubbleText.SetActive(false);
            _thisGameObject.SetActive(false);
        }
        else
        {
            thisRelatedButton.onClick.AddListener(TutorialPanelOpened);
        }
    }

    private void TutorialPanelOpened()
    {
        _uIPanelsManager.ControlBlockClicksPanel(true);
        StartCoroutine(TutorialPanelOpenedCoroutine());
    }

    private IEnumerator TutorialPanelOpenedCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (Input.touchCount > 0)
            {
                thisRelatedGameBigBubbleText.transform.DOScale(0, _panelTime).OnComplete(delegate
                {
                    thisRelatedGameBigBubbleText.SetActive(false);
                    _thisGameObject.SetActive(false);
                    _uIPanelsManager.ControlBlockClicksPanel(false);
                    PlayerPrefs.SetInt(_playerPrefsTutorialID, 1);
                });
                break;
            }
        }
    }
}

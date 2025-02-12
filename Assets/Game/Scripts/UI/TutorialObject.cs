using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private Button thisRelatedButton;
    [SerializeField] private GameObject thisRelatedGameBigBubbleText;
    private string _playerPrefsTutorialID = "IsTutorial_";
    private Sequence _bubbleShake;
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
            gameObject.SetActive(false);
        }
        else
        {
            _bubbleShake = DOTween.Sequence();
            _bubbleShake.SetDelay(0.8f);
            _bubbleShake.Append(transform.DORotate(new Vector3(0f, 0f, 10f), 0.15f)) 
                .Append(transform.DORotate(new Vector3(0f, 0f, -10f), 0.15f))
                .Append(transform.DORotate(new Vector3(0f, 0f, 5f), 0.15f))
                .Append(transform.DORotate(new Vector3(0f, 0f, -5f), 0.15f))
                .Append(transform.DORotate(Vector3.zero, 0.15f)) 
                .SetLoops(2, LoopType.Yoyo) 
                .SetEase(Ease.InOutSine);

            _bubbleShake.SetLoops(-1, LoopType.Restart); 
            thisRelatedButton.onClick.AddListener(TutorialPanelOpened);
        }
    }

    private void OnDisable()
    {
        _bubbleShake.Kill();
    }

    private void TutorialPanelOpened()
    {
        _uIPanelsManager.ControlBlockClicksPanel(true);
        
        StartCoroutine(TutorialPanelOpenedCoroutine());
    }

    private IEnumerator TutorialPanelOpenedCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _uIPanelsManager.GetControlBlockClicksPanelButton().onClick.AddListener(FinishTutorialOpened);
    }

    private void FinishTutorialOpened()
    {
        thisRelatedGameBigBubbleText.transform.DOScale(0, _panelTime).OnComplete(delegate
        {
            _uIPanelsManager.GetControlBlockClicksPanelButton().onClick.RemoveListener(FinishTutorialOpened);
            thisRelatedGameBigBubbleText.SetActive(false);
            thisRelatedButton.onClick.RemoveListener(TutorialPanelOpened);
            _uIPanelsManager.ControlBlockClicksPanel(false);
            PlayerPrefs.SetInt(_playerPrefsTutorialID, 1);
            gameObject.SetActive(false);
        });
    }
}

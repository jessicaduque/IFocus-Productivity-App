using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtra : MonoBehaviour
{
    private Button _thisButton;

    [SerializeField] private bool sensitiveToTransparency = true;
    [SerializeField] private string soundEffectName = "buttonClick";
    private AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(MakeSound);
    }

    private void Start()
    {
        if(sensitiveToTransparency) _thisButton.targetGraphic.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    private void OnEnable()
    {
        _thisButton.enabled = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void MakeSound()
    {
        _audioManager.PlaySfx(soundEffectName);
        _thisButton.enabled = false;
        if(isActiveAndEnabled) StartCoroutine(Reset());
    }        

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);
        _thisButton.enabled = true;
    }

}

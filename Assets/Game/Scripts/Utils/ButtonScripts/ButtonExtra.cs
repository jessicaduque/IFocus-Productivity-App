using System.Collections;
using Game.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtra : MonoBehaviour
{
    [SerializeField] protected Button thisButton;
    [SerializeField] private bool sensitiveToTransparency;
    [SerializeField] private string soundEffectName = "buttonClick";
    private AudioManager _audioManager => AudioManager.I;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (thisButton == null)
        {
            thisButton = GetComponent<Button>();
        }
    }

#endif

    protected virtual void Awake()
    {
        thisButton.onClick.AddListener(MakeSound);
    }

    private void Start()
    {
        if(sensitiveToTransparency) thisButton.targetGraphic.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void MakeSound()
    {
        _audioManager.PlaySfx(soundEffectName);
        if(isActiveAndEnabled) StartCoroutine(Reset());
    }        

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);
    }

}

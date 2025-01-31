using System.Collections;
using Game.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGatinho : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private string soundEffectName = "catMeow";
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

    private void Awake()
    {
        thisButton.onClick.AddListener(MakeSound);
    }

    private void MakeSound()
    {
        _audioManager.PlaySfx(soundEffectName);
        thisButton.enabled = false;
        if (isActiveAndEnabled) StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);
        thisButton.enabled = true;
    }
}

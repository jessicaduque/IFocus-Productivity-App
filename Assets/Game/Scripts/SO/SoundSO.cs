using UnityEngine;
[CreateAssetMenu(fileName = "New Sound", menuName = "Scriptable Objects/Create Sound")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public string artistName;
    public AudioClip clip;
    public bool loop;

    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;

    public float timeSeconds;
}
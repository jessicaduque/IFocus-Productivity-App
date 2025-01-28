using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

namespace Game.Scripts.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Header("Music List")]
        [SerializeField] private List<SoundSO> musicList = new List<SoundSO>();

        private Queue<SoundSO> _musicQueue = new Queue<SoundSO>();
        private AudioManager _audioManager => AudioManager.I;
        private bool _isMusicPlaying = true;

        private void Start()
        {
            SoundSO[] allSounds = Resources.LoadAll<SoundSO>("???");
            musicList.AddRange(allSounds);

            foreach (var music in musicList)
            {
                _musicQueue.Enqueue(music);
            }

            if (_musicQueue.Count > 0)
            {
                PlayNextMusic();
            }
        }

        public void PlayNextMusic()
        {
            if (_musicQueue.Count == 0)
            {
                foreach (var music in musicList)
                {
                    _musicQueue.Enqueue(music);
                }
            }

            if (_isMusicPlaying && _musicQueue.Count > 0)
            {
                SoundSO nextMusic = _musicQueue.Dequeue();
                _audioManager.PlayMusic(nextMusic.soundName, nextMusic.clip, nextMusic.volume, nextMusic.pitch, nextMusic.loop);
                Invoke(nameof(StopAndPlayNextMusic), nextMusic.timeSeconds);
            }
        }

        private void StopAndPlayNextMusic()
        {
            if (_isMusicPlaying)
            {
                _audioManager.StopMusic();
                PlayNextMusic();
            }
        }

        public void PlayMusic(string musicName)
        {
            if (_isMusicPlaying)
            {
                _audioManager.PlayMusic(musicName);
            }
        }

        public void StopMusic()
        {
            _audioManager.StopMusic();
            CancelInvoke(nameof(StopAndPlayNextMusic));
            _isMusicPlaying = false;
            
        }

        public void ToggleMusic()
        {
            _isMusicPlaying = !_isMusicPlaying;
            if (!_isMusicPlaying)
            {
                StopMusic();
            }
            else
            {
                PlayNextMusic();
            }
        }
    }
}

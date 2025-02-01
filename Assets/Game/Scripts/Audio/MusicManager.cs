using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _nextSongButton;
        [SerializeField] private Button _previousSongButton;
        [Header("Music List")]
        private List<SoundSO> musicList = new List<SoundSO>();
        private Queue<SoundSO> _musicQueue = new Queue<SoundSO>();
        private AudioManager _audioManager => AudioManager.I;
        private bool _isMusicPlaying = true;

        private void Start()
        {
            PrepareSongRandomization();
        }

        private void PlayNextMusic()
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
                _audioManager.PlayMusic(nextMusic.soundName);
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

        private void StopMusic()
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
        
        #region RandomizeSongs

        private void PrepareSongRandomization()
        {
            SoundSO[] allSongs = _audioManager.GetSongs();
            
            musicList = ShuffleArray(allSongs).ToList();
            
            foreach (var music in musicList)
            {
                _musicQueue.Enqueue(music);
            }

            if (_musicQueue.Count > 0)
            {
                PlayNextMusic();
            }
        }
        private SoundSO[] ShuffleArray(SoundSO[] songs)
        {
            for (int i = songs.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (songs[i], songs[j]) = (songs[j], songs[i]);
            }
            return songs;
        }
        
        #endregion
    }
}

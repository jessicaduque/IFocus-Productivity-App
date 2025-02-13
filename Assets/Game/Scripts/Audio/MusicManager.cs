using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _songDetailsText;

        [Header("Music List")]
        private List<SoundSO> musicList = new List<SoundSO>();
        private Queue<SoundSO> _musicQueue = new Queue<SoundSO>();
        private Stack<SoundSO> _previousSongsStack = new Stack<SoundSO>();

        private AudioManager _audioManager => AudioManager.I;
        private SoundSO _currentMusic;
        private bool _isMusicPlaying = true;

        private BlackScreenController _blackScreenController => BlackScreenController.I;
        private void Awake()
        {
            SetupButtons();
        }

        private void Start()
        {
            _blackScreenController.gameStartAction += ControlButtonState;
        }

        private void SetupButtons()
        {
            _playButton.onClick.AddListener(PlayMusic);
            _pauseButton.onClick.AddListener(PauseMusic);
            _nextSongButton.onClick.AddListener(PlayNextMusic);
            _previousSongButton.onClick.AddListener(PlayPreviousMusic);
        }

        private void ControlButtonState()
        {
            if (PlayerPrefs.HasKey("IsMusicPlaying"))
            {
                if (PlayerPrefs.GetInt("IsMusicPlaying") == 1)
                {
                    _playButton.gameObject.SetActive(false);
                    _pauseButton.gameObject.SetActive(true);
                    PrepareSongRandomization();
                }
                else
                {
                    _isMusicPlaying = false;
                    _pauseButton.gameObject.SetActive(false);
                    _playButton.gameObject.SetActive(true);
                }
            }
            else
            {
                PlayerPrefs.SetInt("IsMusicPlaying", 1);
                _playButton.gameObject.SetActive(false);
                _pauseButton.gameObject.SetActive(true);
                PrepareSongRandomization();
            }
        }

        #region Button Callbacks

        private void PlayMusic()
        {
            if (_isMusicPlaying)
                _audioManager.ContinuePausedMusic();
            else
                PrepareSongRandomization();

            _playButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            PlayerPrefs.SetInt("IsMusicPlaying", 1);
        }

        private void PauseMusic()
        {
            _audioManager.PauseMusic();
            _pauseButton.gameObject.SetActive(false);
            _playButton.gameObject.SetActive(true);
            PlayerPrefs.SetInt("IsMusicPlaying", 0);
        }

        private void PlayNextMusic()
        {
            if (_currentMusic != null)
            {
                _previousSongsStack.Push(_currentMusic);
            }

            if (_musicQueue.Count == 0)
            {
                foreach (var music in musicList)
                {
                    _musicQueue.Enqueue(music);
                }
            }

            if (_musicQueue.Count > 0)
            {
                _currentMusic = _musicQueue.Dequeue();
                _songDetailsText.text = $"{_currentMusic.soundName} - {_currentMusic.artistName}";
                _audioManager.PlayMusic(_currentMusic.soundName);
                Invoke(nameof(StopAndPlayNextMusic), _currentMusic.timeSeconds);
            }
        }

        private void PlayPreviousMusic()
        {
            if (_previousSongsStack.Count > 0)
            {
                _musicQueue.Enqueue(_currentMusic);
                _currentMusic = _previousSongsStack.Pop();
                _songDetailsText.text = $"{_currentMusic.soundName} - {_currentMusic.artistName}";
                _audioManager.PlayMusic(_currentMusic.soundName);
            }
        }

        #endregion

        private void StopAndPlayNextMusic()
        {
            if (_isMusicPlaying)
            {
                _audioManager.StopMusic();
            }
            PlayNextMusic();
        }

        #region Randomize Songs 

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

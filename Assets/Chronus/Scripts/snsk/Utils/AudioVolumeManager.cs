using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chronus.Utils
{
    public enum AudioType
    {
        SoundEffect,
        BackGoundMusic
    }

    public static class AudioVolumeManager
    {
        private static Dictionary<AudioType, float> _audioTypeToMultiplyCash = new Dictionary<AudioType, float>();
        private static Dictionary<AudioType, Dictionary<AudioSource, float>> _audioSourceToDefaultVolume = new Dictionary<AudioType, Dictionary<AudioSource, float>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _audioTypeToMultiplyCash.Add(AudioType.SoundEffect, 1.0f);
            _audioTypeToMultiplyCash.Add(AudioType.BackGoundMusic, 1.0f);

            _audioSourceToDefaultVolume.Add(AudioType.SoundEffect, new Dictionary<AudioSource, float>());
            _audioSourceToDefaultVolume.Add(AudioType.BackGoundMusic, new Dictionary<AudioSource, float>());

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene nextScene, LoadSceneMode mode)
        {
            foreach (Dictionary<AudioSource, float> pair in _audioSourceToDefaultVolume.Values)
            {
                pair.Clear();
            }

            AudioSource[] audioSources = Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (AudioSource audioSource in audioSources)
            {
                AudioType type;
                if (audioSource.gameObject.tag == "BGM")
                {
                    type = AudioType.BackGoundMusic;
                }
                else
                {
                    type = AudioType.SoundEffect;
                }

                Dictionary<AudioSource, float> defaultAudioSourceMap = _audioSourceToDefaultVolume[type];
                defaultAudioSourceMap.Add(audioSource, audioSource.volume);

                audioSource.volume *= _audioTypeToMultiplyCash[type];
            }
        }

        public static void SetVolume(float volumeMultiply, AudioType type)
        {
            _audioTypeToMultiplyCash[type] = volumeMultiply;

            foreach (KeyValuePair<AudioSource, float> pair in _audioSourceToDefaultVolume[type])
            {
                pair.Key.volume = pair.Value * volumeMultiply;
            }
        }
    }
}

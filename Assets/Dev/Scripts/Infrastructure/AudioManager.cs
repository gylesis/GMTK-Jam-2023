using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Dev.Scripts.Infrastructure
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<SoundMatch> _audioClipList;
        [SerializeField] private Transform _sorcesContainer;
        [SerializeField] private AudioSource _sourcePrefab;

        private List<AudioSource> _sources = new();

        public static AudioManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        public void PlaySound(SoundType soundType)
        {
            var clip =_audioClipList.First(match => match.Type == soundType).AudioClip;
            var source = GetFreeAudioSource();

            source.clip = clip;
            source.Play();
            Observable.Timer(TimeSpan.FromSeconds(clip.length)).TakeUntilDestroy(this).Subscribe(l =>
            {
                source.Stop();
            });
        }

        private AudioSource GetFreeAudioSource()
        {
            var source = _sources.FirstOrDefault(audioSource => !audioSource.isPlaying);
            if (source) return source;
            
            
            var newSource = Instantiate(_sourcePrefab, _sorcesContainer);
            _sources.Add(newSource);
            return newSource;

        }
    }

    [Serializable]
    class SoundMatch
    {
        [SerializeField] private SoundType _type;
        [SerializeField] private AudioClip _audioClip;

        public SoundType Type => _type; 
        public AudioClip AudioClip => _audioClip;
    }

    public enum SoundType
    {
        Activate,
        Checkpoint,
        Deactivate,
        Death,
        Jump,
        JumpPad,
        Platform,
        Land
    }
}
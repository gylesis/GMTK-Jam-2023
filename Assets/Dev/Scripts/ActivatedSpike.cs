using System;
using Dev.Scripts.Characters;
using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts
{
    public class ActivatedSpike : InteractionObject, InterfaceTrackedSpike
    {
        [SerializeField] private bool _active;

        [SerializeField] private Transform _lid1;
        [SerializeField] private Transform _lid2;
        [SerializeField] private Transform _spikes;
        private Tween _openSequence;

        
        public Transform Transform => transform;
        public Collider Collider => GetComponent<Collider>();

        public bool Active => _active;
        
        private void Awake()
        {
            _openSequence = DOTween.Sequence()
                .Append(_lid1.DOLocalRotate(90 * Vector3.up, 0.5f))
                .Join(_lid2.DOLocalRotate(-90 * Vector3.up, 0.5f))
                .Append(_spikes.DOLocalMoveY(1.4f, 0.25f));

            _openSequence.SetAutoKill(false);
            _openSequence.Pause();

            AnimateOpen();
        }

        public override void OnDown()
        {
            Toggle();
        }
        
        public void Toggle()
        {
            if (_openSequence.IsPlaying()) return;
            
            _active = !_active;
            AnimateOpen();
        }

        private void AnimateOpen()
        {
            if (_active)
            {
                _openSequence.PlayForward();
            }
            else
            {
                _openSequence.PlayBackwards();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_active) return;
            if (other.TryGetComponent(out Character character))
            {
                character.Die();
            }
        }
    }
}
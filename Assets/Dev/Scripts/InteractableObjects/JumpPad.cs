using Dev.Scripts.Characters;
using Dev.Scripts.Infrastructure;
using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts.InteractableObjects
{
    public class JumpPad : InteractionObject
    {
        [SerializeField] private bool _active;
        [SerializeField] private float _jumpMultiplier;
        

        [SerializeField] private Transform _lid1;
        [SerializeField] private Transform _lid2;
        [SerializeField] private Transform _pad;

        private Tween _openSequence;
        private bool _initialState;
        
        private void Awake()
        {
            _initialState = _active;
            _openSequence = DOTween.Sequence()
                .Append(_lid1.DOLocalRotate(90 * Vector3.up, 0.5f))
                .Join(_lid2.DOLocalRotate(90 * Vector3.up, 0.5f))
                .Append(_pad.DOLocalMoveZ(-0.5f, 0.25f));

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
            _active = !_active;

            AnimateOpen();
            PlaySound();
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

        private void PlaySound()
        {
            AudioManager.Instance.PlaySound(_active ? SoundType.Activate : SoundType.Deactivate);
        }

        public override void OnReset()
        {
            _active = _initialState;
            
            AnimateOpen();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_active) return;
            
            if (other.TryGetComponent(out Character character))
            {
                character.Jump(_jumpMultiplier, true);
                AudioManager.Instance.PlaySound(SoundType.JumpPad);
                _pad.DOLocalMoveZ(1, 0.25f).SetEase(Ease.OutBounce).SetLoops(2, LoopType.Yoyo);
            }
        }
    }
}
using Dev.Scripts.Characters;
using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts.InteractableObjects
{
    public class JumpPad : InteractionObject
    {
        [SerializeField] private bool _active;

        [SerializeField] private Transform _lid1;
        [SerializeField] private Transform _lid2;
        [SerializeField] private Transform _pad;

        private Tween _openSequence; 
        
        private void Awake()
        {
            _openSequence = DOTween.Sequence()
                .Append(_lid1.DOLocalRotate(90 * Vector3.up, 0.5f))
                .Join(_lid2.DOLocalRotate(-90 * Vector3.up, 0.5f))
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
                character.Jump(1.5f);
            }
        }
    }
}
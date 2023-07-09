using System;
using Dev.Scripts.Infrastructure;
using Dev.Scripts.UI;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpStrength;
        [SerializeField] private float _gravityForceStrength;
        [SerializeField] private LayerMask _ignoreMask;
        [SerializeField] private Transform _checkGroundPoint;

        [SerializeField] private Transform _visuals;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private bool _allowToMove = true;
        

        private bool _grounded;
        private bool _previouslyGrounded = true;

        private bool _ableToCheck = true;
        private bool _checkForDeathCollision = false;
        private InterfaceTrackedPlatform _currentPlatform;
        private LevelManager _levelManager;
        private Tween _flyAnimation;

        public LayerMask IgnoreMask => _ignoreMask;
        public bool Grounded => _grounded;
        public InterfaceTrackedPlatform CurrentPlatform => _currentPlatform;
        public Action OnLanded;

        [Inject]
        private void Init(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            CheckGround();
            SetSpeed();
        }
        

        private void Initialize()
        {
            _flyAnimation = _visuals.DOLocalRotate(-90 * Vector3.forward, 0.2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental)
                .SetAutoKill(false).Pause();

            SetSpeed();
            Observable.Timer(TimeSpan.FromMilliseconds(1000)).TakeUntilDestroy(this).Subscribe(l =>
            {
                _checkForDeathCollision = true;
            });
        }

        public void ActivateMovement(bool active)
        {
            _allowToMove = active;
        }
        
        private void SetSpeed()
        {
            if (!_allowToMove) _rigidbody.velocity = Vector3.zero;
            else
            {
                if (_rigidbody.velocity.x < 0.1f && _checkForDeathCollision)
                {
                    Die();
                }
                else if (_rigidbody.velocity.x < (_rigidbody.transform.right * _speed).magnitude)
                {
                    _rigidbody.velocity = _rigidbody.transform.right * _speed;
                }
            }
        }
        
        private void ApplyGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForceStrength);
        }

        public void Jump(float forceMultiplier = 1, bool prikol = false)
        {
            if(!_grounded && !prikol) return;

            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            _rigidbody.velocity = velocity;
            
            _rigidbody.AddForce(Vector3.up * _jumpStrength * forceMultiplier);
            AudioManager.Instance.PlaySound(SoundType.Jump);
            AnimateFly();
            VFXManager.Instance.PlayJump();
            VFXManager.Instance.SetRunningActive(false);

            _grounded = false;
            _currentPlatform = null;
            _ableToCheck = false;
            Observable.Timer(TimeSpan.FromMilliseconds(200)).TakeUntilDestroy(this).Subscribe(l =>
            {
                _ableToCheck = true;
            });
        }

        private void AnimateFly()
        {
            _flyAnimation.Play();
        }

        private void StopAnimateFly()
        {
            _flyAnimation.Goto(0);
            _flyAnimation.Pause();
        }
        
        private void CheckGround()
        {
            if (!_ableToCheck) return;
            _grounded = Physics.Raycast(_checkGroundPoint.position, Vector3.down, out RaycastHit hit, 0.08f, _ignoreMask);

            //Landed
            if (!_previouslyGrounded && _grounded)
            {
                AudioManager.Instance.PlaySound(SoundType.Land);
                StopAnimateFly();
                
                VFXManager.Instance.PlayLand();
                VFXManager.Instance.SetRunningActive(true);
            }
            
            if (hit.transform)
            {
                Debug.Log(hit.transform.name);
                _currentPlatform = hit.transform.TryGetComponent(out Platform platform) ? platform : (hit.transform.TryGetComponent(out StaticPlatform staticPlatform) ? staticPlatform : null);
                OnLanded.Invoke();
            }

            _previouslyGrounded = _grounded;
        }

        public void Die()
        {
            ActivateMovement(false);
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            AudioManager.Instance.PlaySound(SoundType.Death);
            VFXManager.Instance.PlayDeath();

            Observable.Timer(TimeSpan.FromMilliseconds(1000)).TakeUntilDestroy(this).Subscribe(l =>
            {
                _levelManager.ResetLevel();
            });
        }
    }
}
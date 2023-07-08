﻿using System;
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
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private bool _allowToMove = true;
        

        private bool _grounded;
        private bool _ableToCheck = true;
        private bool _checkForDeathCollision = false;
        private InterfaceTrackedPlatform _currentPlatform;
        private LevelManager _levelManager;

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
                if (_rigidbody.velocity.sqrMagnitude < 0.1f && _checkForDeathCollision)
                {
                    Die();
                }
                else if (_rigidbody.velocity.magnitude < (_rigidbody.transform.right * _speed).magnitude)
                {
                    _rigidbody.velocity = _rigidbody.transform.right * _speed;
                }
            }
        }
        
        private void ApplyGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForceStrength);
        }

        public void Jump(float forceMultiplier = 1)
        {
            if(!_grounded) return;

            var velocity = _rigidbody.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            _rigidbody.velocity = velocity;
            
            _rigidbody.AddForce(Vector3.up * _jumpStrength * forceMultiplier);
            _grounded = false;
            _currentPlatform = null;
            _ableToCheck = false;
            Observable.Timer(TimeSpan.FromMilliseconds(200)).TakeUntilDestroy(this).Subscribe(l =>
            {
                _ableToCheck = true;
            });
        }

        private void CheckGround()
        {
            if (!_ableToCheck) return;
            _grounded = Physics.Raycast(_checkGroundPoint.position, Vector3.down, out RaycastHit hit, 0.1f, _ignoreMask);
            
            if (hit.transform)
            {
                Debug.Log(hit.transform.name);
                _currentPlatform = hit.transform.TryGetComponent(out Platform platform) ? platform : (hit.transform.TryGetComponent(out StaticPlatform staticPlatform) ? staticPlatform : null);
                OnLanded.Invoke();
            }
        }

        public void Die()
        {
            ActivateMovement(false);
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce);
            
            Observable.Timer(TimeSpan.FromMilliseconds(1000)).TakeUntilDestroy(this).Subscribe(l =>
            {
                _levelManager.ResetLevel();
            });
        }
    }
}
using System;
using Dev.Scripts.UI;
using UniRx;
using UnityEngine;

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

        private bool _grounded;
        private bool _ableToCheck = true;
        private InterfaceTrackedPlatform _currentPlatform;

        public LayerMask IgnoreMask => _ignoreMask;
        public bool Grounded => _grounded;
        public InterfaceTrackedPlatform CurrentPlatform => _currentPlatform;
        public Action OnLanded;
        
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
        }

        private void SetSpeed()
        {
            if (_rigidbody.velocity.magnitude < (_rigidbody.transform.right * _speed).magnitude)
            {
                _rigidbody.velocity = _rigidbody.transform.right * _speed;
            }
        }
        
        private void ApplyGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForceStrength);
        }

        public void Jump(float forceMultiplier = 1)
        {
            if(!_grounded) return;
            
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
            if (!_ableToCheck || _grounded) return;
            _grounded = Physics.Raycast(_checkGroundPoint.position, Vector3.down, out RaycastHit hit, 0.1f, _ignoreMask);
            
            if (hit.transform)
            {
                Debug.Log(hit.transform.name);
                _currentPlatform = hit.transform.TryGetComponent(out Platform platform) ? platform : (hit.transform.TryGetComponent(out StaticPlatform staticPlatform) ? staticPlatform : null);
            }
            
            OnLanded.Invoke();
        }

        public void Die()
        {
            /*Restart level*/
        }
    }
}
using System;
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

        public LayerMask IgnoreMask => _ignoreMask;
        public bool Grounded => _grounded;
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

        public void Jump()
        {
            if(!_grounded) return;
            
            _rigidbody.AddForce(Vector3.up * _jumpStrength);
            _grounded = false;

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
            
            OnLanded.Invoke();
            
            if(hit.transform) Debug.Log(hit.transform.name);
        }

        public void Die()
        {
            /*Restart level*/
        }
    }
}
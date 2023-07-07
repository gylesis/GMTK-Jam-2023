using System;
using UnityEngine;

namespace Dev.Scripts.Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpStrength;
        [SerializeField] private float _gravityForceStrength;
        
        [SerializeField] private Rigidbody _rigidbody;
        
        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            ApplyGravity();
        }

        private void Initialize()
        {
            _rigidbody.velocity = _rigidbody.transform.right * _speed;
        }

        private void ApplyGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForceStrength);
        }
        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up * _jumpStrength);
        }
        
    }
}
using System;
using UniRx;
using UnityEngine;

namespace Dev.Scripts
{
    public class Platform : InteractionObject, InterfaceTrackedPlatform
    {
        public Transform Transform => transform;
        public Collider Collider => GetComponent<Collider>();

        [SerializeField] private GameObject _moveEffect;

        protected override void Awake()
        {
            base.Awake();
            
            _moveEffect.gameObject.SetActive(false);
        }

        public override void OnSwipe(Vector3 direction)
        {
            base.OnSwipe(direction);

            _moveEffect.transform.up = -direction;
            _moveEffect.gameObject.SetActive(true);

            Observable.Timer(TimeSpan.FromSeconds(0.25f)).Subscribe((l =>
            {
                _moveEffect.gameObject.SetActive(false);
            }));
        }
    }
}
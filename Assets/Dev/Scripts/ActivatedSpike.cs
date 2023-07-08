using System;
using Dev.Scripts.Characters;
using UnityEngine;

namespace Dev.Scripts
{
    public class ActivatedSpike : InteractionObject, InterfaceTrackedSpike
    {
        public Transform Transform => transform;
        public Collider Collider => GetComponent<Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Character character))
            {
                character.Die();
            }
        }
    }
}
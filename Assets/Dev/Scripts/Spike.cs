using System;
using Dev.Scripts.Characters;
using UnityEngine;

namespace Dev.Scripts
{
    public class Spike : InteractionObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Character character))
            {
                character.Die();
            }
        }
    }
}
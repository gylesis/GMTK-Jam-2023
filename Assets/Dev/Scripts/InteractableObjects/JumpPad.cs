using Dev.Scripts.Characters;
using UnityEngine;

namespace Dev.Scripts.InteractableObjects
{
    public class JumpPad : InteractionObject
    {
        private bool _active;

        public override void OnDown()
        {
            Toggle();
        }

        public void Toggle()
        {
            _active = !_active;
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
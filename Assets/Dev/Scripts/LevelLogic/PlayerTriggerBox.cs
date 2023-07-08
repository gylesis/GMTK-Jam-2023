using UnityEngine;

namespace Dev.Scripts
{
    public class PlayerTriggerBox : TriggerBox
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                base.OnTriggerEnter(other);
            }
        }
    }
}
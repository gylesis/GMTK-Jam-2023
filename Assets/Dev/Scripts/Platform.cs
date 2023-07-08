
using UnityEngine;

namespace Dev.Scripts
{
    public class Platform : InteractionObject, InterfaceTrackedPlatform
    {
        public Transform Transform => transform;
        public Collider Collider => GetComponent<Collider>();
    }
}
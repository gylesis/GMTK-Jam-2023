using UnityEngine;

namespace Dev.Scripts.UI
{
    public class StaticPlatform : MonoBehaviour, InterfaceTrackedPlatform
    {
        public Transform Transform => transform;
        public Collider Collider => GetComponent<Collider>();
    }
}
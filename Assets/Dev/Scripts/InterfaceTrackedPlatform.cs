using UnityEngine;

namespace Dev.Scripts
{
    public interface InterfaceTrackedPlatform
    {
        Transform Transform { get; }
        Collider Collider { get; }
    }
}
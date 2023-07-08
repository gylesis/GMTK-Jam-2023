using UnityEngine;

namespace Dev.Scripts
{
    public interface InterfaceTrackedSpike
    {
        Transform Transform { get; }
        Collider Collider { get; }
    }
}
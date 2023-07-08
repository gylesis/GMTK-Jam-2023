using UnityEngine;

namespace Dev.Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private TriggerBox _finishZone;
        
        public TriggerBox FinishZone => _finishZone;

        public Transform StartPoint => _startPoint;

    }
}
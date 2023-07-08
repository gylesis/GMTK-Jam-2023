using UnityEngine;

namespace Dev.Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;

        public Transform StartPoint => _startPoint;
    }
}
using UnityEngine;
using Zenject;

namespace Dev.Scripts.Infrastructure
{
    public class PortalToLevel : MonoBehaviour
    {
        [SerializeField] private int _level;
        private LevelManager _levelManager;
        
        [Inject]
        private void Init(LevelManager levelManager)
        {
            _levelManager.LoadLevel(_level);
        }
    }
}
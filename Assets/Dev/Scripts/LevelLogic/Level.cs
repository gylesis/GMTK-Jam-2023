using UnityEngine;

namespace Dev.Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelSavePoint[] _savePoints;  
        
        [SerializeField] private SpawnPoint _startPoint;
        [SerializeField] private PlayerTriggerBox _finishZone;
        
        public PlayerTriggerBox FinishZone => _finishZone;

        public SpawnPoint StartPoint => _startPoint;

        public LevelSavePoint[] SavePoints => _savePoints;

        private void OnTransformChildrenChanged()
        {
            _savePoints = GetComponentsInChildren<LevelSavePoint>();
        }
    }
}
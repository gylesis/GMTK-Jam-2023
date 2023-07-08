using UnityEngine;

namespace Dev.Scripts
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelSavePoint[] _savePoints;  
        
        [SerializeField] private SpawnPoint _startPoint;
        [SerializeField] private PlayerTriggerBox _finishZone;
        private InteractionObject[] _interactionObjects;

        public PlayerTriggerBox FinishZone => _finishZone;
        public SpawnPoint StartPoint => _startPoint;
        public LevelSavePoint[] SavePoints => _savePoints;

        public InteractionObject[] InteractionObjects => _interactionObjects;

        private void Awake()
        {
            _interactionObjects = GetComponentsInChildren<InteractionObject>();
        }

        private void OnTransformChildrenChanged()
        {
            _savePoints = GetComponentsInChildren<LevelSavePoint>();
        }
    }
}
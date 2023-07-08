using UniRx;
using UnityEngine;

namespace Dev.Scripts
{
    public class LevelSavePoint : MonoBehaviour
    {
        [SerializeField] private SpawnPoint _spawnPoint;
        [SerializeField] private PlayerTriggerBox _triggerBox;

        public PlayerTriggerBox TriggerBox => _triggerBox;

        private void Awake()
        {
            _triggerBox.TriggerEntered.TakeUntilDestroy(this).Subscribe((collider1 =>
            {
               
            }));
        }

        public Vector3 GetSpawnPos()
        {
            return _spawnPoint.GetPos();
        }
        
    }
}
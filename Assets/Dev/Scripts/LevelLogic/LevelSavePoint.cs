using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Dev.Scripts
{
    public class LevelSavePoint : MonoBehaviour
    {
        [SerializeField] private SpawnPoint _spawnPoint;
        [SerializeField] private PlayerTriggerBox _triggerBox;

        [SerializeField] private Transform _zoneEffect; 
        
        [SerializeField] private Transform _flag;
        
        public PlayerTriggerBox TriggerBox => _triggerBox;

        private void Awake()
        {
            _triggerBox.TriggerEntered.TakeUntilDestroy(this).Subscribe((collider1 =>
            {
                _flag.transform.DOLocalRotate(-90*Vector3.forward, 0.5f).SetEase(Ease.InOutBounce);
                _triggerBox.gameObject.SetActive(false);
                _zoneEffect.gameObject.SetActive(false);
            }));
        }

        public Vector3 GetSpawnPos()
        {
            return _spawnPoint.GetPos();
        }
        
    }
}
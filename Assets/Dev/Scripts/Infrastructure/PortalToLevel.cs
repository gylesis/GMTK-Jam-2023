using Dev.Scripts.UI;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.Infrastructure
{
    public class PortalToLevel : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private TriggerBox _triggerBox;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _blockingPlatform;


        private LevelManager _levelManager;
        private Curtain _curtain;

        [Inject]
        private void Init(LevelManager levelManager, Curtain curtain)
        {
            _levelManager = levelManager;
            _curtain = curtain;
        }

        private void Awake()
        {
            if (_level == 1)
            {
                _blockingPlatform.gameObject.SetActive(false);
            }
            else
            {
                 bool active = PlayerPrefs.GetInt("Level_" + _level) == 0;
                 _blockingPlatform.gameObject.SetActive(active);
            }
            
            _text.text = _level.ToString();
            _triggerBox.TriggerEntered.TakeUntilDestroy(_triggerBox).Subscribe((collider =>
            {
                _levelManager.LoadLevel(_level);
                _curtain.FadeInOut();
            }));
        }
    }
}
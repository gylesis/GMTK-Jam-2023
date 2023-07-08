using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.UI
{
    public class LevelChooseMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _playButton;
        private LevelManager _levelManager;

        protected override void Awake()
        {
            base.Awake();
            _playButton.Clicked.TakeUntilDestroy(this).Subscribe((unit => OnPlayButtonClicked()));
        }

        [Inject]
        private void Init(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        
        private void OnPlayButtonClicked()
        {
            _levelManager.LoadLevel(1);
        }
        
    }
}
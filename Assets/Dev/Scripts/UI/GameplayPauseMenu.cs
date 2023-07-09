using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.UI
{
    public class GameplayPauseMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _quitButton;
        private LevelManager _levelManager;

        public DefaultReactiveButton QuitButton => _quitButton;


        [Inject]
        private void Init(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        
        protected override void Awake()
        {
            base.Awake();

            _quitButton.Clicked.TakeUntilDestroy(this).Subscribe((unit =>
            {
                OnQuitButtonClicked();
            }));
        }

        private void OnQuitButtonClicked()
        {
            _levelManager.LoadMainMenu();
        }
        
    }
}
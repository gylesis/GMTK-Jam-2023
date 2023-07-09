using UniRx;
using UnityEngine;

namespace Dev.Scripts.UI
{
    public class GameplayMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _menuButton;

        protected override void Awake()
        {
            _menuButton.Clicked.TakeUntilDestroy(this).Subscribe((unit =>
            {
                OnPauseButtonClicked();
            }));
        }

        private void OnPauseButtonClicked()
        {
            PauseGame();
            
            MenuSwitcher.Instance.TryGetMenu<GameplayPauseMenu>(out var pauseMenu);

            Hide();
            pauseMenu.Show();
            
            pauseMenu.QuitButton.Clicked.TakeUntilDestroy(this).Subscribe((unit =>
            {
                UnPauseGame();
            }));
            
            pauseMenu.OnSucceedButtonClicked((() =>
            {
                Show();
                pauseMenu.Hide();
                UnPauseGame();
            }));
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void UnPauseGame()
        {
            Time.timeScale = 1;
        }
    }
}
using UniRx;
using UnityEngine;

namespace Dev.Scripts.UI
{
    public class MainMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _playButton;
        [SerializeField] private DefaultReactiveButton _settingsButton;

        protected override void Awake()
        {
            _playButton.Clicked.TakeUntilDestroy(this).Subscribe((unit => OnPlayButtonClicked()));
            _settingsButton.Clicked.TakeUntilDestroy(this).Subscribe((unit => OnSettingsButtonClicked()));
        }

        private void OnPlayButtonClicked()
        {
            var getMenu = MenuSwitcher.Instance.TryGetMenu<LevelChooseMenu>(out var levelChooseMenu);

            if (getMenu)
            {
                levelChooseMenu.OnSucceedButtonClicked((() =>
                {
                    levelChooseMenu.Hide();
                    Show();
                }));
                
                levelChooseMenu.Show();
            }
        }

        private void OnSettingsButtonClicked()
        {
            
        }
        
    }
}
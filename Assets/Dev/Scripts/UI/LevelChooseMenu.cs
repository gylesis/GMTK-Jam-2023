using UniRx;
using UnityEngine;

namespace Dev.Scripts.UI
{
    public class LevelChooseMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _playButton;

        protected override void Awake()
        {
            base.Awake();
            _playButton.Clicked.TakeUntilDestroy(this).Subscribe((unit => OnPlayButtonClicked()));
        }
        
        private void OnPlayButtonClicked()
        {
            LevelManager.Instance.LoadLevel(1);
        }
        
    }
}
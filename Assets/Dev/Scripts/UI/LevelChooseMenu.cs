using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.UI
{
    public class LevelChooseMenu : Menu
    {
        [SerializeField] private DefaultReactiveButton _playButton;
        [SerializeField] private LevelUIController _levelUIController;
        
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

        private void Start()
        {
            _levelUIController.SpawnUI();
        }

        private void OnPlayButtonClicked()
        {
            _levelManager.LoadLevel(_levelUIController.ChosenLevel);
        }
    }
}
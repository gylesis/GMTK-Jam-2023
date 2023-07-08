using System;
using Dev.Scripts.Infrastructure;
using UniRx;

namespace Dev.Scripts
{
    public class LevelStateHandler
    {
        private CameraController _cameraController;
        private PlayerSpawner _playerSpawner;
        private GameSettings _gameSettings;

        public LevelStateHandler(CameraController cameraController, PlayerSpawner playerSpawner, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _playerSpawner = playerSpawner;
            _cameraController = cameraController;
        }

        public void PreStartLevel(Level level)
        {
            SpawnPlayer(level);

            if (_playerSpawner.TryGetPlayer(out var character))
            {
                _cameraController.SetTarget(character.transform).StartSequence();
            }
            
            Observable.Timer(TimeSpan.FromSeconds(_gameSettings.DelayBeforeStartLevel)).Subscribe((l =>
            {
                character.ActivateMovement(true);
            }));
        }

        public void CleanLevel(Level level)
        {
            _playerSpawner.RemovePlayer();
        }
        
        private void SpawnPlayer(Level level)
        {
            _playerSpawner.SpawnPlayerOnLevel(level);
            
            if (_playerSpawner.TryGetPlayer(out var character))
            {
                character.ActivateMovement(false);
            }
        }
        
    }
}
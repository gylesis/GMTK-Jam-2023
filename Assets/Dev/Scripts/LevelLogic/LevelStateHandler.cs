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

            HandleCameraSpeed();
        }

        public void RestartLevel(LevelSavePoint savePoint)
        {
            SpawnPlayer(savePoint);

            HandleCameraSpeed();
        }

        private void HandleCameraSpeed()
        {
            if (_playerSpawner.TryGetPlayer(out var character))
            {
                _cameraController.UpdateCameraSpeed(_gameSettings.CameraMoveToSpeed).SetTarget(character.transform).StartSequence();
            }
            
            Observable.Timer(TimeSpan.FromSeconds(_gameSettings.DelayBeforeStartLevel)).Subscribe((l =>
            {
                SetPlayerMovementState(true);
                _cameraController.UpdateCameraSpeed(_gameSettings.CameraDefaultFollowSpeed);
            }));
        }

        public void FinishLevel(Level level)
        {
            SetPlayerMovementState(false);
        }
        
        public void CleanLevel(Level level)
        {
            _playerSpawner.RemovePlayer();
        }
        
        private void SpawnPlayer(Level level)
        {   
            _playerSpawner.SpawnPlayerOnLevel(level);
            
            SetPlayerMovementState(false);
        }

        private void SpawnPlayer(LevelSavePoint levelSavePoint)
        {
            _playerSpawner.SpawnPlayerOnLevel(levelSavePoint);

            SetPlayerMovementState(false);
        }


        private void SetPlayerMovementState(bool isOn)
        {
            if (_playerSpawner.TryGetPlayer(out var character))
            {
                character.ActivateMovement(isOn);
            }
        }

    }
}
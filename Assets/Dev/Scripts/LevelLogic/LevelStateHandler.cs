using System;
using System.Collections.Generic;
using Dev.Scripts.Infrastructure;
using UniRx;
using UnityEngine;

namespace Dev.Scripts
{
    public class LevelStateHandler
    {
        private CameraController _cameraController;
        private PlayerSpawner _playerSpawner;
        private GameSettings _gameSettings;

        private Dictionary<int, Vector3> _objectsOriginData = new Dictionary<int, Vector3>();

        public LevelStateHandler(CameraController cameraController, PlayerSpawner playerSpawner,
            GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _playerSpawner = playerSpawner;
            _cameraController = cameraController;
        }

        public void PreStartLevel(Level level)
        {
            _objectsOriginData.Clear();
            
            foreach (InteractionObject interactionObject in level.InteractionObjects)
            {
                _objectsOriginData.Add(interactionObject.GetInstanceID(), interactionObject.transform.position);
            }
            
            SpawnPlayer(level);

            HandleCameraSpeed();
        }

        public void RestartLevel(Level level)
        {
            SpawnPlayer(level);

            HandleCameraSpeed();
        }

        public void RestartLevel(LevelSavePoint levelSavePoint)
        {
            SpawnPlayer(levelSavePoint);

            HandleCameraSpeed();
        }

        private void HandleCameraSpeed()
        {
            if (_playerSpawner.TryGetPlayer(out var character))
            {
                _cameraController.UpdateCameraSpeed(_gameSettings.CameraMoveToSpeed).SetTarget(character.transform)
                    .StartSequence();
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
            _cameraController.Dispose();
            
            foreach (InteractionObject interactionObject in level.InteractionObjects)
            {
                interactionObject.transform.position = _objectsOriginData[interactionObject.GetInstanceID()];
            }

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
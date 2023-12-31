﻿using System.Linq;
using Dev.Scripts.Infrastructure;
using Dev.Scripts.UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Dev.Scripts
{
    public class LevelManager : IInitializable
    {
        private LevelsContainer _levelsContainer;
        private Level _currentLevel;
        private LevelStateHandler _levelStateHandler;
        private LevelFactory _levelFactory;
        private Curtain _curtain;
        private int _levelNum;

        public Level Level => _currentLevel;

        private LevelSavePoint _lastSavePoint;
        
        [Inject]
        private void Init(LevelsContainer levelsContainer, LevelStateHandler levelStateHandler, LevelFactory levelFactory, Curtain curtain)
        {
            _levelFactory = levelFactory;
            _levelStateHandler = levelStateHandler;
            _levelsContainer = levelsContainer;
            _curtain = curtain;
        }

        public void Initialize() { }

        public void LoadLevel(int level)
        {
            UnLoadCurrentLevel();

            _levelNum = level;

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("LevelsScene");

            loadSceneAsync.completed += OnLoadSceneAsyncOncompleted;

            void OnLoadSceneAsyncOncompleted(AsyncOperation operation)
            {
                loadSceneAsync.completed -= OnLoadSceneAsyncOncompleted;

                LevelStaticData levelStaticData = _levelsContainer.GetLevelDataByLevel(level);

                _currentLevel = _levelFactory.Create(levelStaticData.LevelPrefab);

                _currentLevel.FinishZone.TriggerEntered.TakeUntilDestroy(_currentLevel)
                    .Subscribe((OnFinishZoneTriggered));

                HandleSavePoints(_currentLevel);
                
                if (_currentLevel.SavePoints.Length > 0) _lastSavePoint = _currentLevel.SavePoints.First();
                
                _levelStateHandler.PreStartLevel(_currentLevel);
            }
        }

        public void LoadMainMenu()
        {
            UnLoadCurrentLevel();
            
            SceneManager.LoadSceneAsync("Lobby");
        }
        
        private void HandleSavePoints(Level level)
        {
            foreach (LevelSavePoint savePoint in level.SavePoints)
            {
                savePoint.TriggerBox.TriggerEntered.TakeUntilDestroy(_currentLevel).Subscribe((collider =>
                {
                    OnSavePointReached(savePoint);
                }));
            }
        }

        private void OnSavePointReached(LevelSavePoint savePoint)
        {
            _lastSavePoint = savePoint;
            AudioManager.Instance.PlaySound(SoundType.Checkpoint);
        }

        private void OnFinishZoneTriggered(Collider other)
        {
            _levelStateHandler.FinishLevel(_currentLevel);
            AudioManager.Instance.PlaySound(SoundType.Finish);

            PlayerPrefs.SetInt("CurrentLevel", _levelNum);
            PlayerPrefs.SetInt("Level_" + (_levelNum + 1) , 1);
            PlayerPrefs.Save();
            _curtain.FadeInOut();
            LoadLevel(0);
        }

        public void ResetLevel()
        {
            _levelStateHandler.CleanLevel(_currentLevel);
            
            if (_lastSavePoint)
            {
                _levelStateHandler.RestartLevel(_lastSavePoint);
            }
            else
            {
                _levelStateHandler.RestartLevel(_currentLevel);
            }
        }

        private void UnLoadCurrentLevel()
        {
            if (_currentLevel != null)
            {
                _levelStateHandler.CleanLevel(_currentLevel);
                Object.Destroy(_currentLevel.gameObject);
            }
        }
    }
}
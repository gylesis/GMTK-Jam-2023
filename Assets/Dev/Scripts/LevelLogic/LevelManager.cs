using System.Linq;
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

        public Level Level => _currentLevel;

        private LevelSavePoint _lastSavePoint;
        
        [Inject]
        private void Init(LevelsContainer levelsContainer, LevelStateHandler levelStateHandler,
            LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
            _levelStateHandler = levelStateHandler;
            _levelsContainer = levelsContainer;
        }

        public void Initialize() { }

        public void LoadLevel(int level)
        {
            UnLoadCurrentLevel();

            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("Level1");

            loadSceneAsync.completed += OnLoadSceneAsyncOncompleted;

            void OnLoadSceneAsyncOncompleted(AsyncOperation operation)
            {
                loadSceneAsync.completed -= OnLoadSceneAsyncOncompleted;

                LevelStaticData levelStaticData = _levelsContainer.GetLevelDataByLevel(level);

                _currentLevel = _levelFactory.Create(levelStaticData.LevelPrefab);

                _currentLevel.FinishZone.TriggerEntered.TakeUntilDestroy(_currentLevel)
                    .Subscribe((OnFinishZoneTriggered));

                foreach (LevelSavePoint savePoint in _currentLevel.SavePoints)
                {
                    savePoint.TriggerBox.TriggerEntered.TakeUntilDestroy(_currentLevel).Subscribe((collider =>
                    {
                        OnSavePointReached(savePoint);
                    }));
                }

                _lastSavePoint = _currentLevel.SavePoints.First();
                
                _levelStateHandler.PreStartLevel(_currentLevel);
            }
        }

        private void OnSavePointReached(LevelSavePoint savePoint)
        {
            _lastSavePoint = savePoint;
        }

        private void OnFinishZoneTriggered(Collider other)
        {
            _levelStateHandler.FinishLevel(_currentLevel);

            ResetLevel();
        }

        public void ResetLevel()
        {
            _levelStateHandler.CleanLevel(_currentLevel);
            _levelStateHandler.RestartLevel(_lastSavePoint);
        }

        public void LoadMainMenu()
        {
            LoadInternal("MainMenu");
        }

        private void LoadInternal(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
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
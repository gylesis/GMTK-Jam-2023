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

        [Inject]
        private void Init(LevelsContainer levelsContainer, LevelStateHandler levelStateHandler, LevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
            _levelStateHandler = levelStateHandler;
            _levelsContainer = levelsContainer;
        }

        public void Initialize()
        {
           
        }

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

                _levelStateHandler.PreStartLevel(_currentLevel);
            }
        }

        [ContextMenu(nameof(ResetLevel))]
        public void ResetLevel()
        {
            _levelStateHandler.CleanLevel(_currentLevel);
            
            _levelStateHandler.PreStartLevel(_currentLevel);
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
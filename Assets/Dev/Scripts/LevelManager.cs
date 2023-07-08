using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void LoadLevel(int level)
        {
            LoadInternal("Level1");
        }

        public void LoadMainMenu()
        {
            LoadInternal("MainMenu");
        }

        private void LoadInternal(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }

    public class Curtain : MonoBehaviour
    {
        
    }
    
}
using Dev.Scripts.Characters;
using UnityEngine;

namespace Dev.Scripts
{
    public class PlayerSpawner
    {
        private Character _characterPrefab;
        private Character _spawnedCharacter;
        private PlayerFactory _playerFactory;

        public PlayerSpawner(Character characterPrefab, PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
            _characterPrefab = characterPrefab;
        }

        public bool TryGetPlayer(out Character character)
        {
            character = _spawnedCharacter;

            return _spawnedCharacter != null;
        }

        public void SpawnPlayerOnLevel(Level level)
        {
            Vector3 spawnPos = level.StartPoint.GetPos();

            var playerSpawnContext = new PlayerSpawnContext();

            playerSpawnContext.Pos = spawnPos;
            playerSpawnContext.Prefab = _characterPrefab;

            _spawnedCharacter = _playerFactory.Create(playerSpawnContext);
        }

        public void SpawnPlayerOnLevel(LevelSavePoint levelSavePoint)
        {
            Vector3 spawnPos = levelSavePoint.GetSpawnPos();

            var playerSpawnContext = new PlayerSpawnContext();

            playerSpawnContext.Pos = spawnPos;
            playerSpawnContext.Prefab = _characterPrefab;

            _spawnedCharacter = _playerFactory.Create(playerSpawnContext);
        }


        public void RemovePlayer()
        {
            if (_spawnedCharacter != null)
            {
                Object.Destroy(_spawnedCharacter.gameObject);
            }
        }
    }

    public struct PlayerSpawnContext
    {
        public Character Prefab;
        public Vector3 Pos;
    }
}
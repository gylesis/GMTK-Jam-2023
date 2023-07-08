using Dev.Scripts.Characters;
using UnityEngine;

namespace Dev.Scripts
{
    public class PlayerSpawner
    {
        private Character _characterPrefab;
        private Character _spawnedCharacter;

        public PlayerSpawner(Character characterPrefab)
        {
            _characterPrefab = characterPrefab;
        }


        public bool TryGetPlayer(out Character character)
        {
            character = _spawnedCharacter;

            return _spawnedCharacter != null;
        }
        
        public void SpawnPlayerOnLevel(Level level)
        {
            Transform levelStartPoint = level.StartPoint;

            _spawnedCharacter = Object.Instantiate(_characterPrefab, levelStartPoint.position, Quaternion.identity);
        }

        public void RemovePlayer()
        {
            if (_spawnedCharacter != null)
            {
                Object.Destroy(_spawnedCharacter.gameObject);
            }
        }
        
    }
}
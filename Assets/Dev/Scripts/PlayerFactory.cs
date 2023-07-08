using Dev.Scripts.Characters;
using UnityEngine;
using Zenject;

namespace Dev.Scripts
{
    public class PlayerFactory : PlaceholderFactory<PlayerSpawnContext, Character> { }
    
    public class IPlayerFactory : IFactory<PlayerSpawnContext, Character>
    {
        private DiContainer _container;

        public IPlayerFactory(DiContainer container)
        {
            _container = container;
        }

        public Character Create(PlayerSpawnContext param)
        {
            //_container.InstantiatePrefabForComponent<Character>(param.Prefab, param.Pos);

            return _container.InstantiatePrefabForComponent<Character>(param.Prefab, param.Pos, Quaternion.identity,
                null);
        }
    }
    
}
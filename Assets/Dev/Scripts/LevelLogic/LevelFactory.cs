using Zenject;

namespace Dev.Scripts
{
    public class LevelFactory : PlaceholderFactory<Level, Level> { }

    public class ILevelFactory : IFactory<Level, Level>
    {
        private DiContainer _container;

        public ILevelFactory(DiContainer container)
        {
            _container = container;
        }

        public Level Create(Level param)
        {
            return _container.InstantiatePrefabForComponent<Level>(param);
        }
    }
}
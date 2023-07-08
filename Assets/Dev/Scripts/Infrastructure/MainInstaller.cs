using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Dev.Scripts.Infrastructure
{
    public class MainInstaller : LifetimeScope
    {
        [SerializeField] private GameSettings _gameSettings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameSettings);

            builder.Register<InteractionObjectsPointerHandler>(Lifetime.Singleton);
            builder.Register<MovementConverter>(Lifetime.Singleton);
        }
    }
}
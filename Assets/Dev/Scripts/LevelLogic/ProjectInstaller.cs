using System;
using Dev.Scripts.Characters;
using Dev.Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Dev.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private Character _characterPrefab;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private LevelsContainer _levelsContainer;
        [SerializeField] private CameraContainer _cameraContainer;
        [SerializeField] private LineDrawer _lineDrawer;
        
        public override void InstallBindings()
        {
            Container.BindFactory<Level, Level, LevelFactory>().FromFactory<ILevelFactory>();
            Container.BindFactory<PlayerSpawnContext,Character, PlayerFactory>().FromFactory<IPlayerFactory>();
            
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
            
            Container.Bind<LineDrawer>().FromInstance(_lineDrawer);
            
            Container.Bind<InteractionObjectsPointerHandler>().AsSingle();
            
            Container.Bind<MovementConverter>().AsSingle().NonLazy();
            
            Container.Bind<PlayerSpawner>().AsSingle().WithArguments(_characterPrefab);
            
            Container.Bind<LevelStateHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();

            Container.Bind<GameSettings>().FromInstance(_gameSettings).AsSingle();
            Container.Bind<LevelsContainer>().FromInstance(_levelsContainer).AsSingle();
            Container.Bind<CameraContainer>().FromInstance(_cameraContainer).AsSingle();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(200, 100, 100, 75), "Reset"))
            {
                Container.Resolve<LevelManager>().ResetLevel();
            }
        }
    }
}
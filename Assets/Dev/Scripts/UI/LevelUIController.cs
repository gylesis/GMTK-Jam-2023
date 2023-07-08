using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts.UI
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private LevelUIView _levelUIViewPrefab;
        
        private LevelsContainer _levelsContainer;

        public int ChosenLevel { get; private set; } = 1;

        private List<LevelUIView> _levelUIViews = new List<LevelUIView>();

        [Inject]
        private void Init(LevelsContainer levelsContainer)
        {
            _levelsContainer = levelsContainer;
        }
        
        public void SpawnUI()
        {
            foreach (LevelStaticData data in _levelsContainer._levelStaticDatas)
            {
                LevelUIView levelUIView = Instantiate(_levelUIViewPrefab,_parent);
                levelUIView.Setup(data.Id, data.Description);

                levelUIView.Button.Clicked.TakeUntilDestroy(this).Subscribe((OnLevelUIClicked));
                
                _levelUIViews.Add(levelUIView);
            }
            
            Select(_levelUIViews.First());
        }

        private void OnLevelUIClicked(EventContext<LevelUIView> context)
        {
            LevelUIView levelUIView = context.Value;
            var levelId = levelUIView.Id;

            ChosenLevel = levelId;

            Select(levelUIView);
        }

        private void Select(LevelUIView levelUIView)
        {
            foreach (LevelUIView uiView in _levelUIViews)
            {
                uiView.SetSelection(levelUIView == uiView);
            }
        }
        
    }
}
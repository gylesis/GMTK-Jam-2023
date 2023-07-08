using System.Collections.Generic;
using UnityEngine;

namespace Dev.Scripts
{
    [CreateAssetMenu(menuName = "StaticData/LevelContainer", fileName = "LevelContainer", order = 0)]
    public class LevelsContainer : ScriptableObject
    {
        public List<LevelStaticData> _levelStaticDatas = new List<LevelStaticData>();

        public LevelStaticData GetLevelDataByLevel(int level)
        {
           return _levelStaticDatas[level - 1];
        }
        
        private void OnValidate()
        {
            for (var index = 0; index < _levelStaticDatas.Count; index++)
            {
                _levelStaticDatas[index]._name = $"{index + 1}";
            }
        }
    }
}
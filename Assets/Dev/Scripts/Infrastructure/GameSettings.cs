using UnityEngine;

namespace Dev.Scripts.Infrastructure
{
    [CreateAssetMenu(menuName = "StaticData/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public int MoveUnitLenght = 2;
    }
}
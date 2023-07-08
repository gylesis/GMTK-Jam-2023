using UnityEngine;

namespace Dev.Scripts.Infrastructure
{
    [CreateAssetMenu(menuName = "StaticData/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public float DelayBeforeStartLevel = 0.5f;
        public int MoveUnitLenght = 2;
        public float CameraDefaultFollowSpeed = 1.5f;
        public float CameraMoveToSpeed = 4f;
        public CameraOffset CameraOffset;
    }
}
using System;
using UnityEngine;

namespace Dev.Scripts
{
    public class LobbyLevel : Level
    {
        [SerializeField] private Transform _movingPart;

        private void Start()
        {
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            var position = _movingPart.transform.position;
            position.x += 8 * (currentLevel - 1);
            _movingPart.transform.position = position;
        }
    }
}
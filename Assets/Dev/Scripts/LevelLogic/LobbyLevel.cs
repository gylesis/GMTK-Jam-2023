using System;
using Dev.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Dev.Scripts
{
    public class LobbyLevel : Level
    {
        [SerializeField] private Transform _movingPart;

        private Curtain _curtain;

        [Inject]
        private void Init(Curtain curtain)
        {
            _curtain = curtain;
        }
        
        private void Start()
        {
            _curtain.FadeOut();
            
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            var position = _movingPart.transform.position;
            position.x -= 8 * (currentLevel);
            _movingPart.transform.position = position;
        }
    }
}
using System;
using UnityEngine;

namespace Dev.Scripts.Infrastructure
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _running;
        [SerializeField] private ParticleSystem _jump;
        [SerializeField] private ParticleSystem _land;
        [SerializeField] private ParticleSystem _death;
        

        public static VFXManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void SetRunningActive(bool active)
        {
            if (active)
            {
                _running.Play();
            }
            else
            {
                _running.Stop();   
            }
        }

        public void PlayJump()
        {
            _jump.Play();    
        }

        public void PlayLand()
        {
            _land.Play();
        }
        
        public void PlayDeath()
        {
            _land.Play();
        }
    }
}
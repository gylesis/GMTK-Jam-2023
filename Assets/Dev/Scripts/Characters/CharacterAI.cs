using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dev.Scripts.Characters
{
    public class CharacterAI : MonoBehaviour
    {
        [SerializeField] private Transform _cliffDetectPoint;
        [SerializeField] private float _cliffDetectDistance;

        [SerializeField] private float _spikeDistance;
        
        
        [SerializeField] private Character _character;

        private List<Spike> _spikes;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            CheckForCliff();
        }

        private void Initialize()
        {
            //InitializeSpikes();
        }
        
        private void CheckForCliff()
        {
            var cliffOccured = !Physics.Raycast(_cliffDetectPoint.position, Vector3.down, _cliffDetectDistance,_character.IgnoreMask);
            if (cliffOccured)
            {
                _character.Jump();
            }
        }

        private void InitializeSpikes()
        {
            _spikes = _spikes.OrderBy(spike => GetHorizontalDistanceTo(spike.transform)).ToList();
        }

        private void CheckForSpikes()
        {
            if (_spikes.First().transform )
            {
                
            }  
        }

        private float GetHorizontalDistanceTo(Transform trans4m)
        {
            return _character.transform.position.x - trans4m.position.x;
        }
    }
}
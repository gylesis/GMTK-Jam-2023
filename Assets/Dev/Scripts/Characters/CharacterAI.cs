using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Dev.Scripts.Characters
{
    public class CharacterAI : MonoBehaviour
    {
        [SerializeField] private Transform _cliffDetectPoint;
        [SerializeField] private float _cliffDetectDistance;

        [SerializeField] private float _spikeDistance;
        [SerializeField] private float _upperAngleToCheckSpikes;
        [SerializeField] private float _lowerAngleToCheckSpikes;

        
        
        [SerializeField] private Character _character;

        private Spike _closestSpike;
        private List<Spike> _spikes = new List<Spike>();

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            CheckForCliff();
            CheckForSpikes();
        }

        private void Initialize()
        {
            InitializeSpikes();
            _character.OnLanded += InitializeSpikes;
        }
        
        private void CheckForCliff()
        {
            if (!_character.Grounded) return; 
            var cliffOccured = !Physics.Raycast(_cliffDetectPoint.position, Vector3.down, _cliffDetectDistance,_character.IgnoreMask);
            Debug.DrawRay(_cliffDetectPoint.position, Vector3.down, Color.yellow, 0.5f);
            
            if (cliffOccured)
            {
                _character.Jump();
            }
        }

        private void InitializeSpikes()
        {
            _spikes = FindObjectsOfType<Spike>().ToList();
            _spikes = _spikes.Where(spike => GetHorizontalDistanceTo(spike.transform) > 0).ToList();
            _spikes = _spikes.OrderBy(spike => GetHorizontalDistanceTo(spike.transform)).ToList();

            if (_spikes.Count > 0)
            {
                _closestSpike = _spikes.First();
            }
            else
            {
                _closestSpike = null;
            }

        }

        private void CheckForSpikes()
        {
            if (!_closestSpike) return;
            DrawSpikeCheckRange();
            if (Vector3.Distance(_closestSpike.transform.position, _character.transform.position) > _spikeDistance) return;

            Vector3 toSpike = _closestSpike.transform.position - _character.transform.position;
            float angle = Vector3.SignedAngle(toSpike, Vector3.right, Vector3.back);

            if ( -_lowerAngleToCheckSpikes < angle && angle < _upperAngleToCheckSpikes)
            {
                _character.Jump();
                _spikes.Remove(_closestSpike);
            }
        }

        private float GetHorizontalDistanceTo(Transform trans4m)
        {
            return  trans4m.position.x - _character.transform.position.x;
        }

        private void DrawSpikeCheckRange()
        {
            Debug.DrawLine(_character.transform.position, _closestSpike.transform.position, Color.white, 0.1f);
            Debug.DrawLine(_character.transform.position, _character.transform.position + _spikeDistance * (_closestSpike.transform.position - _character.transform.position).normalized, Color.red, 0.1f);

            var upperAngle = Mathf.Deg2Rad * _upperAngleToCheckSpikes;
            var lowerAngle = Mathf.Deg2Rad * _lowerAngleToCheckSpikes;

            Debug.DrawLine(_character.transform.position,_character.transform.position + _spikeDistance * 
                (Vector3.right * Mathf.Cos(upperAngle) + Vector3.up * Mathf.Sin(upperAngle)), Color.red, 0.1f);
            Debug.DrawLine(_character.transform.position,_character.transform.position + _spikeDistance * 
                (Vector3.right * Mathf.Cos(-lowerAngle) + Vector3.up * Mathf.Sin(-lowerAngle)), Color.red, 0.1f);
        }
        
    }
}
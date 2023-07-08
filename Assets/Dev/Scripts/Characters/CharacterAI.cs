using System.Collections.Generic;
using System.Linq;
using Dev.Scripts.UI;
using UnityEngine;

namespace Dev.Scripts.Characters
{
    public class CharacterAI : MonoBehaviour
    {
        [SerializeField] private Transform _cliffDetectPoint;
        [SerializeField] private float _cliffDetectDistance;

        [Header("Spike Detection Settings")]
        [SerializeField] private float _spikeDistance;
        [SerializeField] private float _upperAngleToCheckSpikes;
        [SerializeField] private float _lowerAngleToCheckSpikes;
        [SerializeField] private bool _debugSpikeRays;

        [Header("Platform Detection Settings")]
        [SerializeField] private float _minimumPlatformDistance;
        [SerializeField] private float _maximumPlatformDistance;
        [SerializeField] private float _upperAngleToCheckPlatforms;
        [SerializeField] private float _lowerAngleToCheckPlatforms;
        [SerializeField] private bool _debugPlatformRays;
        
        
        [Header("Character")]
        [SerializeField] private Character _character;

        private Spike _closestSpike;
        private List<Spike> _spikes = new List<Spike>();

        private InterfaceTrackedPlatform _closestPlatform;
        private List<InterfaceTrackedPlatform> _platforms = new List<InterfaceTrackedPlatform>();

        private void Start()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            //CheckForCliff();
            CheckForSpikes();
            CheckForPlatform();
        }

        private void Initialize()
        {
            InitializeSpikes();
            InitializePlatforms();
            _character.OnLanded += InitializeSpikes;
            _character.OnLanded += InitializePlatforms;
        }

        private void CheckForCliff()
        {
            if (!_character.Grounded) return;
            var cliffOccured = !Physics.Raycast(_cliffDetectPoint.position, Vector3.down, _cliffDetectDistance,
                _character.IgnoreMask);
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

            _closestSpike = _spikes.Count > 0 ? _spikes.First() : null;
        }

        private void InitializePlatforms()
        {
            _platforms = FindObjectsOfType<Platform>().Select(platform => (InterfaceTrackedPlatform)platform).ToList();
            _platforms.AddRange(FindObjectsOfType<StaticPlatform>().Select(platform => (InterfaceTrackedPlatform)platform).ToList());

            _platforms = _platforms.Where(platform => GetHorizontalDistanceTo(platform.Transform) > 0 && platform != _character.CurrentPlatform).ToList();
            _platforms = _platforms.OrderBy(platform => GetHorizontalDistanceTo(platform.Transform)).ToList();
            
            _closestPlatform = _platforms.Count > 0 ? _platforms.First() : null;
        }

        private void CheckForSpikes()
        {
            if (!_closestSpike) return;
            DrawSpikeCheckRange();
            if (Vector3.Distance(_closestSpike.transform.position, _character.transform.position) >
                _spikeDistance) return;

            Vector3 toSpike = _closestSpike.transform.position - _character.transform.position;
            float angle = Vector3.SignedAngle(toSpike, Vector3.right, Vector3.back);

            if (_lowerAngleToCheckSpikes < angle && angle < _upperAngleToCheckSpikes)
            {
                _character.Jump();
                _spikes.Remove(_closestSpike);
            }
        }

        private void CheckForPlatform()
        {
            if (ReferenceEquals(_closestPlatform, null)) return;
            DrawPlatformCheckRange();
            
            Vector3 platformClosestPoint = _closestPlatform.Collider.bounds.ClosestPoint(_character.transform.position);
            Vector3 toPlatform =  platformClosestPoint - _character.transform.position;
            float distanceToPlatform =  toPlatform.magnitude;

            if (_minimumPlatformDistance > distanceToPlatform || distanceToPlatform > _maximumPlatformDistance) return;

            float angle = Vector3.SignedAngle(toPlatform, Vector3.right, Vector3.back);

            if (_lowerAngleToCheckPlatforms < angle && angle < _upperAngleToCheckPlatforms)
            {
                _character.Jump();
                _platforms.Remove(_closestPlatform);
            }
        }

        private float GetHorizontalDistanceTo(Transform trans4m)
        {
            return trans4m.position.x - _character.transform.position.x;
        }

        private void DrawSpikeCheckRange()
        {
            if (!_debugSpikeRays) return;
            
            Debug.DrawLine(_character.transform.position, _closestSpike.transform.position, Color.white);
            Debug.DrawLine(_character.transform.position,
                _character.transform.position + _spikeDistance *
                (_closestSpike.transform.position - _character.transform.position).normalized, Color.red);

            var upperAngle = Mathf.Deg2Rad * _upperAngleToCheckSpikes;
            var lowerAngle = Mathf.Deg2Rad * _lowerAngleToCheckSpikes;

            Debug.DrawLine(_character.transform.position, _character.transform.position + _spikeDistance *
                (Vector3.right * Mathf.Cos(upperAngle) + Vector3.up * Mathf.Sin(upperAngle)), Color.red);
            Debug.DrawLine(_character.transform.position, _character.transform.position + _spikeDistance *
                (Vector3.right * Mathf.Cos(lowerAngle) + Vector3.up * Mathf.Sin(lowerAngle)), Color.red);
        }
        
        private void DrawPlatformCheckRange()
        {
            if (!_debugPlatformRays) return;
            
            Debug.DrawLine(_character.transform.position, _closestPlatform.Transform.position, Color.white);
            Debug.DrawLine(_character.transform.position + _minimumPlatformDistance * (_closestPlatform.Transform.position - _character.transform.position).normalized,
                _character.transform.position + _maximumPlatformDistance * (_closestPlatform.Transform.position - _character.transform.position).normalized, Color.red);

            var upperAngle = Mathf.Deg2Rad * _upperAngleToCheckPlatforms;
            var lowerAngle = Mathf.Deg2Rad * _lowerAngleToCheckPlatforms;

            Debug.DrawLine(_character.transform.position, _character.transform.position + _spikeDistance *
                (Vector3.right * Mathf.Cos(upperAngle) + Vector3.up * Mathf.Sin(upperAngle)), Color.red);
            Debug.DrawLine(_character.transform.position, _character.transform.position + _spikeDistance *
                (Vector3.right * Mathf.Cos(lowerAngle) + Vector3.up * Mathf.Sin(lowerAngle)), Color.red);
        }
    }
}
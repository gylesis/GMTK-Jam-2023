using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dev.Scripts.Infrastructure;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dev.Scripts
{
     public class CameraController : IInitializable
    {
        private readonly GameSettings _gameSettings;

        private readonly Queue<CameraTarget> _cameraTargets = new Queue<CameraTarget>();
        private IDisposable _movementLockDisposable;
        private IDisposable _distanceConditionDisposable;

        private bool _needToResetCameraSpeed;

        private Action _onComplete;
        private CancellationToken _cancellationToken = new CancellationToken();

        public bool IsOnHold { get; set; }

        private float _speed;
        private CameraContainer _cameraContainer;

        public CameraController(CameraContainer cameraContainer, GameSettings gameSettings)
        {
            _cameraContainer = cameraContainer;
            _gameSettings = gameSettings;
        }

        public void Initialize()
        {
            _speed = _gameSettings.CameraDefaultFollowSpeed;
            _cameraContainer.gameObject.SetActive(false);
        }

        public CameraController SetTarget(Transform transform, float timeToHold = 1)
        {
            CameraTarget cameraTarget = new CameraTarget();

            cameraTarget.HoldTime = timeToHold;
            cameraTarget.Transform = transform;

            _cameraTargets.Enqueue(cameraTarget);

            return this;
        }

        public CameraController SetTarget(CameraTarget cameraTarget)
        {
            _cameraTargets.Enqueue(cameraTarget);

            return this;
        }

        public CameraController OnComplete(Action onComplete)
        {
            _onComplete = onComplete;

            return this;
        }

        /*public CameraController ReturnToOriginAfterComplete()
        {
            _needToReturnToOrigin = true;

            return this;
        }*/

        public CameraController UpdateCameraSpeed(float speed, bool needToResetAfterRelock = true)
        {
            _speed = speed;
            _needToResetCameraSpeed = needToResetAfterRelock;

            return this;
        }

        public async void StartSequence()
        {
            if (_cameraTargets.Count == 0) return;

            _cameraContainer.gameObject.SetActive(true);
            
            _movementLockDisposable?.Dispose();
            _distanceConditionDisposable?.Dispose();

            //  Debug.Log($"camera targets count {_cameraTargets.Count}");

            int count = _cameraTargets.Count;

            for (int i = 1; i <= count; i++)
            {
                if (IsOnHold)
                {
                    while (IsOnHold)
                    {
                        await Task.Yield();
                    }
                }

                CameraTarget cameraTarget = _cameraTargets.Dequeue();

                bool isSequence = count > 1;

                await LockOnTarget(cameraTarget.Transform, null, isSequence);

                //Debug.Log($"reached {cameraTarget.Transform.name}");

                await Task.Delay(TimeSpan.FromSeconds(cameraTarget.HoldTime), _cancellationToken);
            }

            bool isCancelled = _cancellationToken.IsCancellationRequested;

            if (isCancelled == false)
            {
                _onComplete?.Invoke();

                /*if (_needToReturnToOrigin)
                {
                    LockOnTarget(_player.Transform);
                }*/
            }

            _onComplete = null;
           // _needToReturnToOrigin = false;
        }

        private async Task LockOnTarget(Transform transform, Action onReach = null, bool isSequence = false)
        {
            _movementLockDisposable?.Dispose();
            _distanceConditionDisposable?.Dispose();

            bool isReached = false;

            _movementLockDisposable = Observable.EveryLateUpdate().Subscribe((l =>
            {
                Transform camera = _cameraContainer.Transform;

                Vector3 target = transform.position;

                target.x += _gameSettings.CameraOffset.XOffset;
                target.y += _gameSettings.CameraOffset.YOffset;
                target.z += _gameSettings.CameraOffset.ZOffset;

                _speed = _gameSettings.CameraDefaultFollowSpeed;
                
                camera.position = Vector3.Lerp(camera.position, target, _speed * Time.deltaTime);
            }));

            _distanceConditionDisposable = Observable.EveryUpdate().Subscribe((l =>
            {
                Transform camera = _cameraContainer.Transform;

                Vector3 target = transform.position;

                target.x += _gameSettings.CameraOffset.XOffset;
                target.y += _gameSettings.CameraOffset.YOffset;
                target.z += _gameSettings.CameraOffset.ZOffset;

                float distance = (camera.position - target).sqrMagnitude;

                if (distance <= 3)
                {
                    // Debug.Log($"Lock completed");
                    //_movementLockDisposable.Dispose();
                    isReached = true;
                    onReach?.Invoke();
                    _distanceConditionDisposable?.Dispose();
                }
            }));

            while (true)
            {
                if (isReached)
                {
                    if (_needToResetCameraSpeed)
                    {
                        _speed = _gameSettings.CameraDefaultFollowSpeed;
                        _needToResetCameraSpeed = false;
                    }

                    await Task.CompletedTask;
                    break;
                }

                await Task.Delay(1, _cancellationToken);
            }
        }

        public void Dispose()
        {
            _cancellationToken.ThrowIfCancellationRequested();
            _movementLockDisposable?.Dispose();
            _distanceConditionDisposable?.Dispose();
        }
    }

     [Serializable]
    public class CameraTarget
    {
        public Transform Transform;
        [Range(1f, 5f)] public float HoldTime = 1f;
    }
}
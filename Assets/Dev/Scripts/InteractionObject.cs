using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dev.Scripts
{
    public abstract class InteractionObject : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private int _moveUnits;
        [SerializeField] private List<SwipeDirection> _allowedSwipeDirection;
        [SerializeField] private bool _ableToReset = true;

        public List<SwipeDirection> AllowedSwipeDirection => _allowedSwipeDirection;

        public int MoveUnits => _moveUnits; 

        private Color _originColor;

        public bool IsMoving { get; set; }

        public bool AbleToReset => _ableToReset;

        protected virtual void Awake()
        {
            if (_allowedSwipeDirection.Count == 0)
            {
                _allowedSwipeDirection = new List<SwipeDirection>()
                {
                    SwipeDirection.Left,
                    SwipeDirection.Right,
                    SwipeDirection.Up,
                    SwipeDirection.Down
                };
            }
            
            _renderer = GetComponent<Renderer>();

            if (_renderer != null)
            {
                _originColor = _renderer.material.color;
            }
        }

        public virtual void OnSwipe(Vector3 direction) { }

        public virtual void OnUp() { }

        public virtual void OnDown() { }

        public void SetColor(Color color)
        {
            if (_renderer != null)
            {
                _renderer.material.color = color;
            }
        }

        public void SetOriginColor()
        {
            SetColor(_originColor);
        }

        /*public void SetMaterial(Material material)
        {
            _renderer.material = material;
        }*/
        public virtual void OnReset() {}
    }

    public interface ICommand
    {
        void Do(InteractionObject interactionObject);
    }


    public class SwipeCommand : ICommand
    {
        private Vector3 _swipeDirection;
        private int _units;

        public SwipeCommand(Vector3 swipeDirection, int units = 1)
        {
            _units = units;
            _swipeDirection = swipeDirection;
        }

        public void Do(InteractionObject interactionObject)
        {
            Vector3 targetPos = interactionObject.transform.position + _swipeDirection * _units;

            interactionObject.transform.DOMove(targetPos, 0.5f).SetEase(Ease.Linear);
        }
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
}
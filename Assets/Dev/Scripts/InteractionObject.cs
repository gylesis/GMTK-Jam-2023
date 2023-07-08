using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts
{
    public abstract class InteractionObject : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private Color _originColor;

        public bool IsMoving { get; set; }

        public virtual void OnSwipe(Vector3 direction) { }

        public virtual void OnUp() { }

        public virtual void OnDown() { }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _originColor = _renderer.material.color;
        }

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }

        public void SetOriginColor()
        {
            SetColor(_originColor);
        }

        /*public void SetMaterial(Material material)
        {
            _renderer.material = material;
        }*/
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
}
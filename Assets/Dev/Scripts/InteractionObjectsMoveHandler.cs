using Dev.Scripts.Infrastructure;
using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts
{
    public class InteractionObjectsPointerHandler
    {
        private InteractionObject _selectedObject;
        private Vector2 _mouseDownPos;
        
        private MovementConverter _movementConverter = new MovementConverter();

        private GameSettings _gameSettings;

        public InteractionObjectsPointerHandler(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public void Down(InteractionObject interactionObject)
        {
            _mouseDownPos = Input.mousePosition;
            
            interactionObject.OnDown();
            interactionObject.SetColor(Color.yellow);
        }

        public void Up(InteractionObject interactionObject)
        {
            interactionObject.OnUp();
            
            Vector2 up = Input.mousePosition;
            Vector2 swipeDirection = up - _mouseDownPos;

            var magnitude = swipeDirection.magnitude;

            if (magnitude < 50f) return;

            if (interactionObject.IsMoving) return;

            float moveUnits = interactionObject.MoveUnits;
            swipeDirection = swipeDirection.GetStraightDirection();

            Vector3 origin = interactionObject.transform.position;
            Vector3 targetPos = interactionObject.transform.position +
                                new Vector3(swipeDirection.x, swipeDirection.y, 0) * moveUnits;

            var hasPath = _movementConverter.HasPath(origin, targetPos, interactionObject.transform, swipeDirection, moveUnits);

            _mouseDownPos = Vector2.zero;

            if (hasPath == false)
            {
                Debug.Log($"NO PATH THIS WAY!");
                return;
            }

            Swipe(interactionObject, targetPos, swipeDirection);
        }

        private void Swipe(InteractionObject interactionObject, Vector3 targetPos, Vector2 swipeDirection)
        {
            interactionObject.OnSwipe(swipeDirection);

            interactionObject.IsMoving = true;
            interactionObject.transform.DOMove(targetPos, 0.2f).OnComplete((() => interactionObject.IsMoving = false));
        }
    }
    
}
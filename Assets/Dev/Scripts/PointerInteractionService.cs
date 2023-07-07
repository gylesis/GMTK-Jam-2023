using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts
{
    public class PointerInteractionService : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionObjLayerMask;
        [SerializeField] private float _hitRadius = 0.5f;

        [SerializeField] private float _moveUnits = 2f;
        [SerializeField] private float _deadZoneThreshold = 50f;
        
        private InteractionObject _selectedObject;

        private Vector2 _downPos = Vector2.down;

        private MovementConverter _movementConverter = new MovementConverter();
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) )
            {
                PointerDownHandle();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                PointerUpHandle();
            }

            if (_downPos != Vector2.zero && _selectedObject != null)
            {
                OnDrag(_selectedObject);
            }
        }

        private void PointerDownHandle()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycast = Physics.SphereCast(ray, _hitRadius, out var hit, 99, _interactionObjLayerMask);

            if (raycast == false)
            {
                UnSelectCurrentInteractionObj();
                
               // Debug.Log($"No object for raycast!");
                return;
            }

            var hasInteraction = hit.collider.TryGetComponent<InteractionObject>(out var interactionObject);

            if (hasInteraction)
            {
                _downPos = Input.mousePosition;
              //  Debug.Log($"Has interaction component");
                OnDown(interactionObject);
            }
            else
            {
               // Debug.Log($"Does not have interaction component");
            }
        }

        private void PointerUpHandle()
        {
            if (_selectedObject == null) return;

            OnSwipe(_selectedObject);
        }

        private void OnDrag(InteractionObject interactionObject)
        {
            Vector2 up = Input.mousePosition;
            Vector2 direction = up - _downPos;
            
            direction = GetStraightDirection(direction);

            Vector3 origin = interactionObject.transform.position;
            Vector3 targetPos = origin + new Vector3(direction.x, direction.y, 0) * _moveUnits;

            var hasPath = _movementConverter.HasPath(origin, targetPos, interactionObject.transform);

            Debug.DrawRay(origin, direction, Color.blue);
            
            Debug.Log($"Has Path {hasPath}");
        }

        private void OnSwipe(InteractionObject interactionObject)
        {
            Vector2 up = Input.mousePosition;
            Vector2 direction = up - _downPos;

            var magnitude = direction.magnitude;

            if (magnitude < _deadZoneThreshold) return;
            
            if(interactionObject.IsMoving) return;
            
            direction = GetStraightDirection(direction);

            interactionObject.OnSwipe(direction);

            Vector3 targetPos = interactionObject.transform.position + new Vector3(direction.x, direction.y, 0) * _moveUnits;

            interactionObject.IsMoving = true;
            interactionObject.transform.DOMove(targetPos, 0.2f).OnComplete((() => interactionObject.IsMoving = false));
            
            //Debug.DrawRay(interactionObject.transform.position, direction, Color.black, 2f);
            
            _downPos = Vector2.zero;
            
           // Debug.Log($"Swipe Direction {direction}");
        }

        private void OnDown(InteractionObject interactionObject)
        {
            UnSelectCurrentInteractionObj();

            _selectedObject = interactionObject;

            _selectedObject.OnDown();
            
            _selectedObject.SetColor(Color.yellow);
        }

        private Vector2 GetStraightDirection(Vector2 rawDirection)
        {
            rawDirection.Normalize();
            
            Vector2 direction;

            if (rawDirection.y > 0.5f )
            {
                direction = Vector2.up;
            }
            else if (rawDirection.x > 0.5f)
            {
                direction = Vector2.right;
            }
            else if (rawDirection.x < -0.5f)
            {
                direction = Vector2.left;
            }
            else if (rawDirection.y < -0.5f)
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.zero;
            }

            return direction;
        }

        private void UnSelectCurrentInteractionObj()
        {
            if (_selectedObject != null)
            {
                _selectedObject.SetOriginColor();
                _selectedObject = null;
            } 
        }
        
    }
}
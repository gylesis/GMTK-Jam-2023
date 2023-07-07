using DG.Tweening;
using UnityEngine;

namespace Dev.Scripts
{
    public class PointerInteractionService : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionObjLayerMask;
        [SerializeField] private float _hitRadius = 2f;

        [SerializeField] private float _moveUnits = 2f;
        
        private InteractionObject _selectedObject;

        private Vector2 _downPos;

        private void Update()
        {
            PointerDownHandle();
            PointerUpHandle();
        }

        private void PointerUpHandle()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CalculateSwipe();
            }
        }

        private void PointerDownHandle()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) == false) return;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycast = Physics.SphereCast(ray, _hitRadius, out var hit, 99, _interactionObjLayerMask);

            if (raycast == false)
            {
                Debug.Log($"No object for raycast!");
                return;
            }

            var hasInteraction = hit.collider.TryGetComponent<InteractionObject>(out var interactionObject);

            if (hasInteraction)
            {
                _downPos = Input.mousePosition;
                Debug.Log($"Has interaction component");
                OnInteractionObjectDown(interactionObject);
            }
            else
            {
                Debug.Log($"Does not have interaction component");
            }
        }


        private void CalculateSwipe()
        {
            if (_selectedObject == null) return;

            OnInteractionObjectSwipe();
        }

        private void OnInteractionObjectSwipe()
        {
            Vector2 up = Input.mousePosition;
    
            Vector2 direction = up - _downPos;
            direction = GetDirection(direction);

            _selectedObject.OnSwipe(direction);

            Vector3 targetPos = _selectedObject.transform.position + new Vector3(direction.x, direction.y, 0) * _moveUnits;
            
            _selectedObject.transform.DOMove(targetPos, 0.5f);
            
            Debug.DrawRay(_selectedObject.transform.position, direction, Color.black, 2f);
            
            Debug.Log($"Swipe Direction {direction}");
        }

        private Vector2 GetDirection(Vector2 rawDirection)
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

        private void OnInteractionObjectDown(InteractionObject interactionObject)
        {
            UnselectCurrentInteractionObj();

            _selectedObject = interactionObject;

            _selectedObject.OnDown();
            
            _selectedObject.SetColor(Color.yellow);
        }

        private void UnselectCurrentInteractionObj()
        {
            if (_selectedObject != null)
            {
                _selectedObject.SetOriginColor();
            } 
        }
    }
}
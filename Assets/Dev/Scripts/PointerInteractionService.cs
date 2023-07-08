using Dev.Scripts.Infrastructure;
using UnityEngine;
using Zenject;

namespace Dev.Scripts
{
    public class PointerInteractionService : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionObjLayerMask;
        [SerializeField] private float _hitRadius = 0.5f;

        private InteractionObject _selectedObject;

        private Vector2 _downPos = Vector2.down;
        private MovementConverter _movementConverter;
        private InteractionObjectsPointerHandler _objectsPointerHandler;
        private GameSettings _gameSettings;
        private LineDrawer _lineDrawer;
        private CameraContainer _cameraContainer;

        [Inject]
        private void Init(MovementConverter movementConverter, InteractionObjectsPointerHandler objectsPointerHandler, GameSettings gameSettings, LineDrawer lineDrawer, CameraContainer cameraContainer)
        {
            _cameraContainer = cameraContainer;
            _lineDrawer = lineDrawer;
            _gameSettings = gameSettings;
            _objectsPointerHandler = objectsPointerHandler;
            _movementConverter = movementConverter;
        }
            
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
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
            Ray ray = _cameraContainer.Camera.ScreenPointToRay(Input.mousePosition);

            bool raycast = Physics.SphereCast(ray, _hitRadius, out var hit, 99, _interactionObjLayerMask);

            if (raycast == false)
            {
                TryUnSelectCurrentInteractionObj();

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
            _lineDrawer.StopDrawing();
            
            if (_selectedObject == null) return;

            OnUp(_selectedObject);
        }

        private void OnDrag(InteractionObject interactionObject)
        {
            Vector2 up = Input.mousePosition;
            Vector2 direction = up - _downPos;

            direction = direction.GetStraightDirection();

            Vector3 origin = interactionObject.transform.position;
            Vector3 targetPos = origin + new Vector3(direction.x, direction.y, 0) * _gameSettings.MoveUnitLenght;

            float offset;

            if (direction == Vector2.up || direction == Vector2.down)
            {
                offset = (interactionObject.transform.localScale.y / 2f);
            }
            else
            {
                offset = (interactionObject.transform.localScale.x / 2f);
            }
            
            Vector3 rayOrigin = origin + (Vector3) direction * offset;
            
            var hasPath = _movementConverter.HasPath(origin, targetPos, interactionObject.transform, direction);

            if (hasPath)
            {
                _lineDrawer.DrawLine(rayOrigin, targetPos);
            }
            else
            {
                _lineDrawer.StopDrawing();
            }
        }

        private void OnUp(InteractionObject interactionObject)
        {
            _objectsPointerHandler.Up(interactionObject);
        }

        private void OnDown(InteractionObject interactionObject)
        {
            TryUnSelectCurrentInteractionObj();

            _selectedObject = interactionObject;

            _objectsPointerHandler.Down(interactionObject);
        }

        private void TryUnSelectCurrentInteractionObj()
        {
            if (_selectedObject != null)
            {
                _selectedObject.SetOriginColor();
                _selectedObject = null;
            }
        }
    }
}
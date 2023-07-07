using UnityEngine;

namespace Dev.Scripts
{
    public class PointerInteractionService : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionObjLayerMask; 
        
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycast = Physics.SphereCast(ray, 2f,out var hit, 99,_interactionObjLayerMask);

            if (raycast)
            {
                Debug.Log($"Hit {hit.collider}", hit.collider);
            }
        }
    }

   
    
}
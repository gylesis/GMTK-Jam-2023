using UnityEngine;

namespace Dev.Scripts
{
    public class MovementConverter
    {
        public bool HasPath(Vector3 origin, Vector3 target, Transform sender, Vector2 rawDirection, float moveUnits)
        {
            float offset;

            if (rawDirection == Vector2.up || rawDirection == Vector2.down)
            {
                offset = (sender.transform.localScale.y / 2f);
            }
            else
            {
                offset = (sender.transform.localScale.x / 2f);
            }
            
            Vector3 rayDirection = (target - origin).normalized;
            Vector3 rayOrigin = origin + rayDirection * offset;

            var hits = Physics.RaycastAll(rayOrigin, rayDirection, moveUnits * 2);

            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    bool notSelf = hit.collider.GetInstanceID() != sender.GetInstanceID();
                    
                    if (notSelf)
                    {
                        target = hit.point;
                        break;
                    }
                }
            }
            else
            {
                return true;
            }
            
            Debug.DrawLine(rayOrigin, target, Color.red);

            Vector3 direction = (target - rayOrigin);
            
            float magnitude = direction.magnitude;

            return magnitude >= moveUnits;
        }   
        
    }
}
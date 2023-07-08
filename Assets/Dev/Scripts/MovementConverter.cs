using UnityEngine;

namespace Dev.Scripts
{
    public class MovementConverter
    {
        public bool HasPath(Vector3 origin, Vector3 target, Transform sender, float moveUnits = 2f)
        {
            Vector3 rayDirection = (target - origin).normalized;
            Vector3 rayOrigin = origin + rayDirection * (sender.transform.localScale.x / 2f);

            var hits = Physics.RaycastAll(rayOrigin, rayDirection, moveUnits);

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
            
            Debug.DrawLine(rayOrigin, target, Color.red);

            Vector3 direction = (target - rayOrigin);
            
            float magnitude = direction.magnitude;

            Debug.Log($"magnitude {magnitude}");
                
            return magnitude <= moveUnits;
        }   
        
    }
}
using UnityEngine;

namespace Dev.Scripts
{
    public class SpawnPoint : MonoBehaviour
    {
        private Vector3 _cachedPoint = Vector3.back;
        
        public Vector3 GetPos()
        {
            if (_cachedPoint == Vector3.back)
            {
                var sphereCast = Physics.SphereCast(transform.position, 0.1f, Vector3.down, out var hit);
                
                if (sphereCast)
                {
                    _cachedPoint = hit.point;
                }
                else
                {
                    _cachedPoint = transform.position;
                }
                
            }


            return _cachedPoint;
        }
    }
}
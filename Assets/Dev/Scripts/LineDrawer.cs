using System.Collections.Generic;
using UnityEngine;

namespace Dev.Scripts
{
    public class LineDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        private List<Vector3> _points = new List<Vector3>();

        public void DrawLine(Vector3 origin, Vector3 targetPos)
        {
            StopDrawing();
            
            _points.Add(origin);
            _points.Add(targetPos);

            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }

        public void StopDrawing()
        {
            if(_points.Count == 0) return;
            
            _points.Clear();

            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }

    }
}
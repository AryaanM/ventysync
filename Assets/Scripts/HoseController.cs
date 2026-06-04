using System.Collections.Generic;
using UnityEngine;

public class HoseController : MonoBehaviour
{
    public Transform startPoint;  // Pre-assigned Start GameObject
    public Transform endPoint;    // Pre-assigned End GameObject
    public int segmentCount = 10; // Number of segments (smoothness)

    private LineRenderer lineRenderer;
    private List<Vector3> hosePositions = new List<Vector3>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
    }

    void Update()
    {
        UpdateHose();
    }

    void UpdateHose()
    {
        hosePositions.Clear();

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);  // Interpolation factor (0 to 1)
            Vector3 point = Vector3.Lerp(startPoint.position, endPoint.position, t);
            point.y -= Mathf.Sin(t * Mathf.PI) * 0.5f;  // Simulates a slight sag (gravity effect)
            hosePositions.Add(point);
        }

        lineRenderer.SetPositions(hosePositions.ToArray());
    }
}

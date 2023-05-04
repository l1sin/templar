using UnityEngine;

public class Tracer : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _decreaseWidth = 1;

    private void Update()
    {
        _lineRenderer.widthMultiplier -= _decreaseWidth * Time.deltaTime;
        if (_lineRenderer.widthMultiplier <= 0)
        {
            Destroy(gameObject);
        }
    }

    public static LineRenderer Trace(GameObject tracerPrefab, Transform shootingPoint, Vector2 endpos)
    {
        var trace = Instantiate(tracerPrefab, shootingPoint.position, Quaternion.identity);
        var lineRenderer = trace.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, endpos);
        return lineRenderer;
    }

    public static LineRenderer TraceReflect(GameObject tracerPrefab, Transform shootingPoint, Vector2 hitPoint, Vector3 normal, float reflectLength)
    {
        GameObject trace = Instantiate(tracerPrefab, hitPoint, Quaternion.identity);
        LineRenderer lineRenderer = trace.GetComponent<LineRenderer>();
        Vector2 reflectionDirection = Vector2.Reflect(hitPoint - (Vector2)shootingPoint.position, normal).normalized;
        float reflectionLength = Random.Range(0, reflectLength);
        lineRenderer.SetPosition(1, reflectionDirection * reflectionLength);
        return lineRenderer;
    }
}

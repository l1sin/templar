using UnityEngine;

public class Trace : MonoBehaviour
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
}

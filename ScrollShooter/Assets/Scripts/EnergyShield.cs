using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    [SerializeField] private GameObject _energyShield;
    [SerializeField] private Transform _pivot;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _distanceMulitplier;
    [SerializeField] public bool IsActive;
    [SerializeField] private Renderer _energyShieldRenderer;

    private void Update()
    {
        RotateAndPos();
        Render();
    }

    private void Render()
    {
        if (!Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetMouseButton(2))
        {
            IsActive = true;
            _energyShieldRenderer.enabled = true;
        }
        else
        {
            IsActive = false;
            _energyShieldRenderer.enabled = false;
        }
    }

    private void RotateAndPos()
    {
        var mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        var difference = mousePos - _pivot.position;
        var RotationZRad = Mathf.Atan2(difference.normalized.y, difference.normalized.x);
        var RotationZDeg = RotationZRad * Mathf.Rad2Deg;

        _energyShield.transform.rotation = Quaternion.Euler(0, 0, RotationZDeg);
        _energyShield.transform.position = new Vector2(Mathf.Cos(RotationZRad), Mathf.Sin(RotationZRad)) * _distanceMulitplier + (Vector2)_pivot.position;
    }
}

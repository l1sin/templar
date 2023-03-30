using UnityEngine;

public class AimGun : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] public static Vector3 MousePosOffset;
    [SerializeField] private Vector3 _cameraMovementScale;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private bool _moveCameraToCursor;

    [HideInInspector] private Vector3 _mousePos;
    [HideInInspector] private Vector3 _mousePosVeiwport;
    [HideInInspector] private Vector3 _difference;
    [HideInInspector] private float _rotationZ; 

    private void Update()
    {
        RotateGun();
        if (_moveCameraToCursor)
        {
            MoveCameraToCursor();
        }
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        _difference = _mousePos - transform.position;
        _rotationZ = Mathf.Atan2(_difference.normalized.y, _difference.normalized.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ);
        if (_difference.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }
        else if (_difference.x < 0)
        {
            transform.rotation = Quaternion.Euler(180, 0, -_rotationZ);
        }
    }

    private void MoveCameraToCursor()
    {
        _mousePosVeiwport = Camera.main.WorldToViewportPoint(_mousePos);
        MousePosOffset = MoveViewportZeroToCenter(_mousePosVeiwport);
        _cam.transform.localPosition = new Vector3 (MousePosOffset.x * _cameraMovementScale.x, MousePosOffset.y * _cameraMovementScale.y, _cam.transform.position.z) + _cameraOffset;
    }

    private Vector3 MoveViewportZeroToCenter(Vector3 ViewPortCoordinates)
    {
        float x = ViewPortCoordinates.x * 2 - 1;
        float y = ViewPortCoordinates.y * 2 - 1;
        float z = ViewPortCoordinates.z;

        return new Vector3(x, y, z);
    }
}

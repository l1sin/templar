using UnityEngine;

public class AimGun : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraMovementScale;
    private GameObject _cam;
    private Vector3 _mousePos;
    private Vector3 _mousePosVeiwport;
    public static Vector3 _mousePosOffset;
    private Vector3 _difference;
    private float _rotationZ;

    [SerializeField] private bool _moveCameraToCursor;

    private void Awake()
    {
        _cam = Camera.main.gameObject;
    }

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
        _mousePosOffset = MoveViewportZeroToCenter(_mousePosVeiwport);
        _cam.transform.localPosition = new Vector3 (_mousePosOffset.x * _cameraMovementScale.x, _mousePosOffset.y * _cameraMovementScale.y, _cam.transform.position.z);
    }

    private Vector3 MoveViewportZeroToCenter(Vector3 ViewPortCoordinates)
    {
        float x = ViewPortCoordinates.x * 2 - 1;
        float y = ViewPortCoordinates.y * 2 - 1;
        float z = ViewPortCoordinates.z;

        return new Vector3(x, y, z);
    }
}

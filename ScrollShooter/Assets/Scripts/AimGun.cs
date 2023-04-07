using UnityEngine;

public class AimGun : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] public static Vector3 MousePosOffset;
    [SerializeField] private Vector3 _cameraMovementScale;
    [SerializeField] private Vector3 _cameraOffset;
    
    [SerializeField] private bool _moveCameraToCursor;
    [SerializeField] private BodyController _bodyController;

    [SerializeField] private Transform _centerAimPos;

    [HideInInspector] private Vector3 _mousePos;
    [HideInInspector] private Vector3 _mousePosVeiwport;
    [HideInInspector] private Vector3 _difference;
    [HideInInspector] public static float RotationZ;

    [SerializeField] private float _headFlex = 30f;
    [SerializeField] private float _headRotationPower;

    private void Update()
    {
        RotateGun();
        RotateHead();
        if (_moveCameraToCursor)
        {
            MoveCameraToCursor();
        }
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        _difference = _mousePos - _centerAimPos.position;
        RotationZ = Mathf.Atan2(_difference.normalized.y, _difference.normalized.x) * Mathf.Rad2Deg;

        _bodyController.LeftHandActiveBackR.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ);
        _bodyController.RightHandActiveFrontR.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ);

        _bodyController.LeftHandActiveFrontL.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ - 180);
        _bodyController.RightHandActiveBackL.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ - 180);

        if (_difference.x > 0)
        {
            ChangeHandsToLookRight();
            ChangeActiveHandWhileLookingRight();
            _bodyController.HeadAndBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_difference.x < 0)
        {
            ChangeHandsToLookLeft();
            ChangeActiveHandWhileLookingLeft();
            _bodyController.HeadAndBody.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void RotateHead()
    {
        if (RotationZ > -_headFlex && RotationZ < _headFlex)
        {
            _bodyController.Head.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ * _headRotationPower);
        }
        else if (RotationZ > 180 - _headFlex || RotationZ < -180 + _headFlex)
        {
            _bodyController.Head.transform.rotation = Quaternion.Euler(180, 0, -RotationZ  * _headRotationPower);
        }
        Debug.Log(RotationZ);
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

    // It just works.

    private void ChangeHandsToLookRight()
    {
        _bodyController.LeftHandActiveBackRRenderer.enabled = true;
        _bodyController.LeftHandInactiveBackRRenderer.enabled = false;
        _bodyController.RightHandActiveFrontRRenderer.enabled = false;
        _bodyController.RightHandInactiveFrontRRenderer.enabled = true;

        _bodyController.LeftHandActiveFrontLRenderer.enabled = false;
        _bodyController.LeftHandInactiveFrontLRenderer.enabled = false;
        _bodyController.RightHandActiveBackLRenderer.enabled = false;
        _bodyController.RightHandInactiveBackLRenderer.enabled = false;
    }

    private void ChangeHandsToLookLeft()
    {
        _bodyController.LeftHandActiveBackRRenderer.enabled = false;
        _bodyController.LeftHandInactiveBackRRenderer.enabled = false;
        _bodyController.RightHandActiveFrontRRenderer.enabled = false;
        _bodyController.RightHandInactiveFrontRRenderer.enabled = false;

        _bodyController.LeftHandActiveFrontLRenderer.enabled = true;
        _bodyController.LeftHandInactiveFrontLRenderer.enabled = false;
        _bodyController.RightHandActiveBackLRenderer.enabled = false;
        _bodyController.RightHandInactiveBackLRenderer.enabled = true;
    }

    private void ChangeActiveHandWhileLookingRight()
    {
        if (Input.GetMouseButton(1))
        {
            _bodyController.LeftHandActiveBackRRenderer.enabled = false;
            _bodyController.LeftHandInactiveBackRRenderer.enabled = true;
            _bodyController.RightHandActiveFrontRRenderer.enabled = true;
            _bodyController.RightHandInactiveFrontRRenderer.enabled = false;
        }
        else
        {
            _bodyController.LeftHandActiveBackRRenderer.enabled = true;
            _bodyController.LeftHandInactiveBackRRenderer.enabled = false;
            _bodyController.RightHandActiveFrontRRenderer.enabled = false;
            _bodyController.RightHandInactiveFrontRRenderer.enabled = true;
        }
    }

    private void ChangeActiveHandWhileLookingLeft()
    {
        if (Input.GetMouseButton(1))
        {
            _bodyController.LeftHandActiveFrontLRenderer.enabled = false;
            _bodyController.LeftHandInactiveFrontLRenderer.enabled = true;
            _bodyController.RightHandActiveBackLRenderer.enabled = true;
            _bodyController.RightHandInactiveBackLRenderer.enabled = false;
        }
        else
        {
            _bodyController.LeftHandActiveFrontLRenderer.enabled = true;
            _bodyController.LeftHandInactiveFrontLRenderer.enabled = false;
            _bodyController.RightHandActiveBackLRenderer.enabled = false;
            _bodyController.RightHandInactiveBackLRenderer.enabled = true;
        }
    }
}

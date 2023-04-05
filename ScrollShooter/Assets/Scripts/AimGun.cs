using UnityEngine;

public class AimGun : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] public static Vector3 MousePosOffset;
    [SerializeField] private Vector3 _cameraMovementScale;
    [SerializeField] private Vector3 _cameraOffset;
    
    [SerializeField] private bool _moveCameraToCursor;
    [SerializeField] private BodyController _bodyController;

    [HideInInspector] private Vector3 _mousePos;
    [HideInInspector] private Vector3 _mousePosVeiwport;
    [HideInInspector] private Vector3 _difference;
    [HideInInspector] private float _rotationZ;

    [HideInInspector] private GameObject _activeHandL;
    [HideInInspector] private GameObject _activeHandR;
    [HideInInspector] public GameObject ActiveHand;

    private void Start()
    {
        _activeHandL = _bodyController.LeftHandActiveFrontL;
        _activeHandR = _bodyController.LeftHandActiveBackR;
        ActiveHand = _activeHandR;
    }

    private void Update()
    {
        ChangeActiveHand();
        RotateGun();
        if (_moveCameraToCursor)
        {
            MoveCameraToCursor();
        }
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        _difference = _mousePos - ActiveHand.transform.position;
        _rotationZ = Mathf.Atan2(_difference.normalized.y, _difference.normalized.x) * Mathf.Rad2Deg; 
        if (_difference.x > 0)
        {
            _bodyController.HeadAndBody.transform.rotation = Quaternion.Euler(0, 0, 0);

            ActiveHand = _activeHandR;
            ActiveHand.transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ);
        }
        else if (_difference.x < 0)
        {
            _bodyController.HeadAndBody.transform.rotation = Quaternion.Euler(0, 180, 0);

            ActiveHand = _activeHandL;
            ActiveHand.transform.rotation = Quaternion.Euler(0f, 0f, _rotationZ - 180);
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

    private void ChangeActiveHand()
    {
        if (Input.GetMouseButton(1))
        {
            _activeHandL = _bodyController.RightHandActiveBackL;
            _activeHandR = _bodyController.RightHandActiveFrontR;

            _bodyController.LeftHandActiveBackR.SetActive(false);
            _bodyController.LeftHandInactiveBackR.SetActive(true);
            _bodyController.RightHandActiveFrontR.SetActive(true);
            _bodyController.RightHandInactiveFrontR.SetActive(false);

            _bodyController.LeftHandActiveFrontL.SetActive(false);
            _bodyController.LeftHandInactiveFrontL.SetActive(true);
            _bodyController.RightHandActiveBackL.SetActive(true);
            _bodyController.RightHandInactiveBackL.SetActive(false);
        }
        else
        {
            _activeHandL = _bodyController.LeftHandActiveFrontL;
            _activeHandR = _bodyController.LeftHandActiveBackR;

            _bodyController.LeftHandActiveBackR.SetActive(true);
            _bodyController.LeftHandInactiveBackR.SetActive(false);
            _bodyController.RightHandActiveFrontR.SetActive(false);
            _bodyController.RightHandInactiveFrontR.SetActive(true);

            _bodyController.LeftHandActiveFrontL.SetActive(true);
            _bodyController.LeftHandInactiveFrontL.SetActive(false);
            _bodyController.RightHandActiveBackL.SetActive(false);
            _bodyController.RightHandInactiveBackL.SetActive(true);
        }
    }
}

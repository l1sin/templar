using UnityEngine;

public class Player : BaseEntity
{
    [SerializeField] public bool _powerUp;
    [SerializeField] private BodyController _bodyController;
    [SerializeField] private Renderer _energyShield;
    [SerializeField] private Material _materialBlue;
    [SerializeField] private Material _materialRed;
    private Material _currentMaterial;

    private void Update()
    {
        if (_powerUp)
        {
            ChangeColorRed();
        }
        else
        {
            ChangeColorBlue();
        }
        Color();
    }

    private void ChangeColorBlue()
    {
        _currentMaterial = _materialBlue;
    }
    private void ChangeColorRed()
    {
        _currentMaterial = _materialRed;
    }

    private void Color()
    {
        _bodyController.HeadRenderer.material = _currentMaterial;
        _bodyController.BodyRenderer.material = _currentMaterial;

        _bodyController.LeftHandActiveBackRRenderer.material = _currentMaterial;
        _bodyController.LeftHandInactiveBackRRenderer.material = _currentMaterial;
        _bodyController.RightHandActiveFrontRRenderer.material = _currentMaterial;
        _bodyController.RightHandInactiveFrontRRenderer.material = _currentMaterial;

        _bodyController.LeftHandActiveFrontLRenderer.material = _currentMaterial;
        _bodyController.LeftHandInactiveFrontLRenderer.material = _currentMaterial;
        _bodyController.RightHandActiveBackLRenderer.material = _currentMaterial;
        _bodyController.RightHandInactiveBackLRenderer.material = _currentMaterial;

        _energyShield.material = _currentMaterial;
    }
}

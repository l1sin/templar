using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _firePeriod;
    [HideInInspector] private float _nextFire;

    private void Start()
    {
        _nextFire = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFire)
        {
            Instantiate(_projectilePrefab, _shootingPoint.position, Quaternion.Euler(transform.localEulerAngles));
            _nextFire = Time.time +_firePeriod;
        }
    }
}

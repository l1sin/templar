using System.Collections.Generic;
using UnityEngine;

public class ParticlesFollow : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
     private Vector3 _center;
     private GameObject[,] _particles;
    [SerializeField] private Vector2 _size;
    [SerializeField] private GameObject _particleSystem;
     private Vector2 _startPoint;
    [SerializeField] private List<GameObject> _systems;

    [SerializeField] private List<GameObject> _l;
    [SerializeField] private List<GameObject> _vC;
    [SerializeField] private List<GameObject> _r;

    [SerializeField] private List<GameObject> _d;
    [SerializeField] private List<GameObject> _hC;
    [SerializeField] private List<GameObject> _u;
    

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        Follow();
    }

    public void Follow()
    {
        if (_followTarget.position.x - _center.x > _size.x / 2)
        {
            _center.x += _size.x;
            MoveRight();
        }
        if (_followTarget.position.y - _center.y > _size.y / 2)
        {
            _center.y += _size.y;
            MoveUp();
        }
        if (_followTarget.position.x - _center.x < -_size.x / 2)
        {
            _center.x -= _size.x;
            MoveLeft();
        }
        if (_followTarget.position.y - _center.y < -_size.y / 2)
        {
            _center.y -= _size.y;
            MoveDown();
        }
    }

    public void CreateParticleSystems()
    {
        Clear();
        _startPoint = -_size;
        _particles = new GameObject[3, 3];
        for (int y = 0; y < _particles.GetLength(1); y++)
        {
            for (int x = 0; x < _particles.GetLength(0); x++)
            {
                Vector2 position;
                position.x = _startPoint.x + _size.x * x;
                position.y = _startPoint.y + _size.y * y;
                GameObject newSystem = Instantiate(_particleSystem, position + (Vector2)transform.position, Quaternion.identity, transform);
                _systems.Add(newSystem);
                _particles[x, y] = newSystem;
            }
        }
        _r = new List<GameObject>();
        _l = new List<GameObject>();
        _u = new List<GameObject>();
        _d = new List<GameObject>();
        _vC = new List<GameObject>();
        _hC = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            _r.Add(_particles[2, i]);
        }
        for (int i = 0; i < 3; i++)
        {
            _vC.Add(_particles[1, i]);
        }
        for (int i = 0; i < 3; i++)
        {
            _l.Add(_particles[0, i]);
        }
        for (int i = 0; i < 3; i++)
        {
            _u.Add(_particles[i, 2]);
        }
        for (int i = 0; i < 3; i++)
        {
            _hC.Add(_particles[i, 1]);
        } 
        for (int i = 0; i < 3; i++)
        {
            _d.Add(_particles[i, 0]);
        }
    }

    private void MoveRight()
    {
        foreach(var i in _l)
        {
            i.transform.Translate(Vector2.right * _size.x * 3);
        }
        List<GameObject> temp = new List<GameObject>(_r);
        _r = new List<GameObject>(_l);
        _l = new List<GameObject>(_vC);
        _vC = new List<GameObject>(temp);
    }
    private void MoveUp()
    {
        foreach (var i in _d)
        {
            i.transform.Translate(Vector2.up * _size.y * 3); 
        }
        List<GameObject> temp = new List<GameObject>(_u);
        _u = new List<GameObject>(_d);
        _d = new List<GameObject>(_hC);
        _hC = new List<GameObject>(temp);
    }
    private void MoveLeft()
    {
        foreach (var i in _r)
        {
            i.transform.Translate(Vector2.left * _size.x * 3);
        }
        List<GameObject> temp = new List<GameObject>(_l);
        _l = new List<GameObject>(_r);
        _r = new List<GameObject>(_vC);
        _vC = new List<GameObject>(temp);
    }
    private void MoveDown()
    {
        foreach (var i in _u)
        {
            i.transform.Translate(Vector2.down * _size.y * 3);
        }
        List<GameObject> temp = new List<GameObject>(_d);
        _d = new List<GameObject>(_u);
        _u = new List<GameObject>(_hC);
        _hC = new List<GameObject>(temp);
    }

    public void Clear()
    {
        foreach (GameObject system in _systems)
        {
            DestroyImmediate(system);
        }
        _systems.Clear();
        _r.Clear();
        _l.Clear();
        _u.Clear();
        _d.Clear();
        _vC.Clear();
        _hC.Clear();
    }
}

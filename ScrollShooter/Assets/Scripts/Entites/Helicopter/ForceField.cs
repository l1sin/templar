using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField][Range(0f, 1f)] private float _alpha;
    [SerializeField][Range(0f, 1f)] private float _phase;
    [HideInInspector] private MaterialPropertyBlock _materialPropertyBlock;
    [SerializeField][GradientUsage(true)] private Gradient _gradient;

    [Header("Stats")]
    [SerializeField] private float _maxForceFieldHealth;
    [SerializeField] private float _currentForceFieldHealth;
    [SerializeField] private float _forceFieldRegenerationTime;

    [SerializeField] private float _ignitionTime;
    

    [SerializeField] private Collider2D _collider2D;


    [HideInInspector] private float _currentAlpha;
    [HideInInspector] private bool _forceFieldEnabled;
    [HideInInspector] private float _forceFieldRegenerationTimer;
    [HideInInspector] private float _ignitionInterpolator;
    [HideInInspector] private bool _ignitionInterpolatorGoUp;
    [HideInInspector] private bool _ignitionComplete;

    private void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (!_forceFieldEnabled) RegenerateForceField();
        Ignite();
    }
    public void GetDamage(float damage)
    {
        _currentForceFieldHealth -= damage;
        _phase = _currentForceFieldHealth / _maxForceFieldHealth;
        _materialPropertyBlock.SetColor("_Color", _gradient.Evaluate(_phase));
        _materialPropertyBlock.SetFloat("_Alpha", _currentAlpha);
        _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        if (_currentForceFieldHealth <= 0) DisableForceField();
    }

    public void DisableForceField()
    {
        _forceFieldEnabled = false;
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _forceFieldRegenerationTimer = _forceFieldRegenerationTime;
    }

    public void EnableForceField()
    {
        _forceFieldEnabled = true;
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;

        StartIgnition();
    }

    private void RegenerateForceField()
    {
        _forceFieldRegenerationTimer -= Time.deltaTime;
        if (_forceFieldRegenerationTimer <= 0) EnableForceField();
    }

    private void Ignite()
    {
        if (!_ignitionComplete)
        {
            if (_ignitionInterpolatorGoUp) _currentAlpha = Mathf.Lerp(_alpha, 1, _ignitionInterpolator);
            else _currentAlpha = Mathf.Lerp(_alpha, _ignitionInterpolator, 1);

            _ignitionInterpolator += Time.deltaTime * _ignitionTime;

            if (_ignitionInterpolator >= 1) _ignitionInterpolatorGoUp = false;
            else if (_ignitionInterpolator <= 0) _ignitionComplete = true;
        } 
    }

    private void StartIgnition()
    {
        _ignitionComplete = false;
        _ignitionInterpolator = 0;
        _ignitionInterpolatorGoUp = true;
    }
}

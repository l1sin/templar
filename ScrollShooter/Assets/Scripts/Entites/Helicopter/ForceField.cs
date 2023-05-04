using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField][Range(0f, 1f)] private float _maxAlpha;
    [SerializeField][Range(0f, 1f)] private float _ignitionAlpha;
    [SerializeField][Range(0f, 1f)] private float _phase;
    [HideInInspector] private MaterialPropertyBlock _materialPropertyBlock;
    [SerializeField][GradientUsage(true)] private Gradient _gradient;

    [Header("Stats")]
    [SerializeField] private float _maxForceFieldHealth;
    [SerializeField] private float _currentForceFieldHealth;
    [SerializeField] private float _forceFieldRegenerationTime;

    [SerializeField] private float _ignitionTime;
    

    [SerializeField] private Collider2D _collider2D;

    [Header("Hidden values")]
    [SerializeField] private float _currentAlpha;
    [SerializeField] private bool _forceFieldEnabled;
    [SerializeField] private float _forceFieldRegenerationTimer;
    [SerializeField] private float _ignitionInterpolator;
    [SerializeField] private bool _ignitionInterpolatorGoUp;
    [SerializeField] private bool _ignitionComplete;

    private void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _currentAlpha = _maxAlpha;
        _ignitionComplete = true;
        _forceFieldEnabled = true;
        _currentForceFieldHealth = _maxForceFieldHealth;
        UpdateColor();
    }

    private void Update()
    {
        if (!_forceFieldEnabled) RegenerateForceField();
        if (!_ignitionComplete) Ignite();

    }
    public void GetDamage(float damage)
    {
        _currentForceFieldHealth -= damage;
        UpdateColor();
        if (_currentForceFieldHealth <= 0) DisableForceField();
    }

    public void DisableForceField()
    {
        _forceFieldEnabled = false;
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        
    }

    public void EnableForceField()
    {
        _forceFieldEnabled = true;
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;
        _forceFieldRegenerationTimer = _forceFieldRegenerationTime;
        _currentForceFieldHealth = _maxForceFieldHealth;
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
            if (_ignitionInterpolator >= 1 && _ignitionInterpolatorGoUp) _ignitionInterpolatorGoUp = false;
            else if (_ignitionInterpolator <= 0 && !_ignitionInterpolatorGoUp) _ignitionComplete = true;

            if (_ignitionInterpolatorGoUp) _ignitionInterpolator += Time.deltaTime * (1 / _ignitionTime);
            else _ignitionInterpolator -= Time.deltaTime * (1 / _ignitionTime);

            _currentAlpha = Mathf.Lerp(_maxAlpha, _ignitionAlpha, _ignitionInterpolator);
            UpdateColor();
        } 
    }

    private void StartIgnition()
    {
        _ignitionComplete = false;
        _ignitionInterpolator = 0;
        _ignitionInterpolatorGoUp = true;
    }

    private void UpdateColor()
    {
        _phase = _currentForceFieldHealth / _maxForceFieldHealth;
        _materialPropertyBlock.SetColor("_Color", _gradient.Evaluate(_phase));
        _materialPropertyBlock.SetFloat("_Alpha", _currentAlpha);
        _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}

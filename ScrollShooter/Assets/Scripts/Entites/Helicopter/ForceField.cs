using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] private float _repulseForce;

    [Header("SFX")]
    [SerializeField] private AudioClip[] _ignitionSounds;
    [SerializeField] private AudioMixerGroup _forceFieldMixerGroup;

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
        _forceFieldRegenerationTimer = _forceFieldRegenerationTime;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collision.transform.position.x - transform.position.x > 0)
            {
                rigidbody2D.AddForce(Vector2.right * _repulseForce, ForceMode2D.Impulse);
            }
            else
            {
                rigidbody2D.AddForce(Vector2.left * _repulseForce, ForceMode2D.Impulse);
            } 
        }
    }

    private void StartIgnition()
    {
        if (_ignitionComplete)
        {
            AudioManager.Instance.MakeSound(transform.position, _ignitionSounds, _forceFieldMixerGroup);
        }
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

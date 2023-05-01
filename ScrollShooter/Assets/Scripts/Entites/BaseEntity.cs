using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] protected float _maxHealthPoints;
    [SerializeField] protected float _currentHealthPoints;
    private float _damageBlinkLength = 0.1f;
    private float _damageBlinkTimer;
    private bool _isDamaged;
    protected Material _currentMaterial;
    [SerializeField] protected Material _damageBlinkMaterial;
    protected Material _tempMaterial;

    protected virtual void Awake()
    {
        _currentHealthPoints = _maxHealthPoints;
    }

    protected virtual void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_isDamaged)
        {
            ResetBlink();
        }
    }

    public virtual void GetDamage(float damage)
    {
        _currentHealthPoints -= damage;
        DamageBlink();
        if (_currentHealthPoints <= 0)
        {
            Die();
        }
    }
    public virtual void GetHeal(float heal)
    {
        _currentHealthPoints += heal;
        if (_currentHealthPoints >= _maxHealthPoints)
        {
            _currentHealthPoints = _maxHealthPoints;
        }
    }

    private void DamageBlink()
    {
        _isDamaged = true;
        if (_currentMaterial != _damageBlinkMaterial) _tempMaterial = _currentMaterial;
        _damageBlinkTimer = _damageBlinkLength;
        _currentMaterial = _damageBlinkMaterial;
        ApplyMaterial();
    }

    private void ResetBlink()
    {
        _damageBlinkTimer -= Time.deltaTime;
        if (_damageBlinkTimer <= 0)
        {
            _currentMaterial = _tempMaterial;
            ApplyMaterial();
            _isDamaged = false;
        }
    }

    protected virtual void ApplyMaterial() { }
    protected virtual void Die() { }
}

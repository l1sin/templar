using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float CurrentHealth;
    private float _damageBlinkLength = 0.1f;
    private float _damageBlinkTimer;
    private bool _isDamaged;
    protected Material CurrentMaterial;
    [SerializeField] protected Material DamageBlinkMaterial;
    protected Material TempMaterial;

    protected virtual void Awake()
    {
        CurrentHealth = MaxHealth;
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
        CurrentHealth -= damage;
        DamageBlink();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void GetHeal(float heal)
    {
        CurrentHealth += heal;
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    private void DamageBlink()
    {
        _isDamaged = true;
        if (CurrentMaterial != DamageBlinkMaterial) TempMaterial = CurrentMaterial;
        _damageBlinkTimer = _damageBlinkLength;
        CurrentMaterial = DamageBlinkMaterial;
        ApplyMaterial();
    }

    private void ResetBlink()
    {
        _damageBlinkTimer -= Time.deltaTime;
        if (_damageBlinkTimer <= 0)
        {
            CurrentMaterial = TempMaterial;
            ApplyMaterial();
            _isDamaged = false;
        }
    }

    protected virtual void ApplyMaterial() { }
    protected virtual void Die() { }
}

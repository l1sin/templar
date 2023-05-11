using UnityEngine;
using UnityEngine.Audio;

public class BaseEntity : MonoBehaviour
{
    [Header("Base Entity Properties")]
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float CurrentHealth;
    [SerializeField] protected Material DamageBlinkMaterial;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioMixerGroup _audioMixer;

    [Header("Hidden Values")]
    [HideInInspector] private float _damageBlinkLength = 0.1f;
    [HideInInspector] private float _damageBlinkTimer;
    [HideInInspector] private bool _isDamaged;
    [HideInInspector] protected Material CurrentMaterial;
    [HideInInspector] protected Material TempMaterial;

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
        AudioManager.Instance.MakeSound(transform, _hitSounds, _audioMixer);
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
    public virtual void Die() { }
}

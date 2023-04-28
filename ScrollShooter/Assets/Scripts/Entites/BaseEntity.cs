using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [SerializeField] protected float _maxHealthPoints;
    [SerializeField] protected float _currentHealthPoints;

    private void Awake()
    {
        _currentHealthPoints = _maxHealthPoints;
    }

    public void GetDamage(float damage)
    {
        _currentHealthPoints -= damage;
        if (_currentHealthPoints <= 0)
        {
            Die();
        }
    }
    public void GetHeal(float damage)
    {
        _currentHealthPoints += damage;
        if (_currentHealthPoints >= _maxHealthPoints)
        {
            _currentHealthPoints = _maxHealthPoints;
        }
    }

    protected virtual void Die()
    {

    }
}

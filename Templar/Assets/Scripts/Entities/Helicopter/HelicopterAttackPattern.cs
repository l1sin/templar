using UnityEngine;

public class HelicopterAttackPattern : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Minigun _minigun;
    [SerializeField] private Lasergun _lasergun;
    [SerializeField] private SpawnMinion _spawnMinion;

    [Header("Common")]
    [SerializeField] private float _startAttackCooldown;

    [Header("Minigun Preferences")]
    [SerializeField] private float _minigunCooldown;
    [SerializeField] private float _minigunAttackLength;

    [Header("Lasergun Preferences")]
    [SerializeField] private float _lasergunCooldown;
    [SerializeField] private float _lasergunAttackLength;

    [Header("Spawn Minion Preferences")]
    [SerializeField] private float _spawnMinionCooldown;

    [Header("Hidden Values")]
    [HideInInspector] private float _attackTimer;
    [HideInInspector] private float _attackCooldownTimer;

    private void Awake()
    {
        ResetWeapons();
        _attackCooldownTimer = _startAttackCooldown;
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        ResetTimers();
        StartAttack();
        StopAttack();
    }

    private void StartAttack()
    {
        if (_attackCooldownTimer <= 0)
        {
            ResetWeapons();

            int randomAttack = Random.Range(0, 3);
            switch (randomAttack)
            {
                case 0:
                    _minigun.enabled = true;
                    _attackCooldownTimer = _minigunCooldown + _minigunAttackLength;
                    _attackTimer = _minigunAttackLength;
                    break;

                case 1:
                    _lasergun.enabled = true;
                    _attackCooldownTimer = _lasergunCooldown + _lasergunAttackLength;
                    _attackTimer = _lasergunAttackLength;
                    break;

                case 2:
                    _spawnMinion.Spawn();
                    _attackCooldownTimer = _spawnMinionCooldown;
                    break;
            }
        }
    }

    private void ResetTimers()
    {
        _attackCooldownTimer -= Time.deltaTime;
        _attackTimer -= Time.deltaTime;
    }

    private void StopAttack()
    {
        if (_attackTimer <= 0)
        {
            ResetWeapons();
        }
    }

    private void ResetWeapons()
    {
        _minigun.enabled = false;
        _lasergun.enabled = false;
        _minigun.ResetThis();
        _lasergun.ResetThis();
    }
}

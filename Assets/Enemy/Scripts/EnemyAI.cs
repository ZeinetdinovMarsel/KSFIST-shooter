using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Weapon _weapon;

    [Header("Patrol Settings")]
    public Vector3 PatrolCenter;
    public float PatrolRadius = 20f;
    public float PatrolWaitTime = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private float _attackRange = 10f;

    private EnemyState _currentState;
    private float _distanceToPlayer;
    private bool _isAttacking;

    private void Start()
    {
        _currentState = new PatrolState(this);
    }

    private void Update()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        _currentState?.UpdateState();
    }

    public void SwitchState(EnemyState newState)
    {
        _currentState = newState;
    }

    public void MoveTo(Vector3 destination)
    {
        if (_agent != null && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(destination);
        }
    }

    public bool IsPlayerInDetectionRange() => _distanceToPlayer <= _detectionRange;

    public bool IsPlayerInAttackRange() => _distanceToPlayer <= _attackRange;

    public void ShootAtPlayer()
    {
        if (_weapon == null || _isAttacking) return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        Vector3 fireDirection = (Player.position - _weapon.transform.position).normalized;
        _weapon.Shoot(_weapon.FirePoint.position, fireDirection);
        yield return new WaitForSeconds(_weapon.FireRate);
        _isAttacking = false;
    }
}

using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyAI enemy) : base(enemy) { }

    public override void UpdateState()
    {
        if (_enemy.IsPlayerInAttackRange())
        {
            _enemy.SwitchState(new AttackState(_enemy));
            return;
        }

        if (!_enemy.IsPlayerInDetectionRange())
        {
            _enemy.SwitchState(new PatrolState(_enemy));
            return;
        }

        _enemy.MoveTo(_enemy.Player.position);
    }
}

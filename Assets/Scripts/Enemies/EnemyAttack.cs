using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackForce;
    [SerializeField] private float attackDuration = 1;
    
    private Enemy _enemy;
    
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_enemy.State == Enemy.EnemyState.Dead)
            return;
        
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            _enemy.ChangeState(Enemy.EnemyState.Attack);
            
            float direction = _enemy.CurrentPlayerPosition.x - transform.position.x > 0 ? 1f : -1f;
            
            EventManager.TriggerEvent(Events.PLAYER_UNDER_ATTACK, new Vector2(attackForce * direction, attackForce));

            StartCoroutine(WaitAndSeek());
        }
    }

    IEnumerator WaitAndSeek()
    {
        yield return new WaitForSeconds(attackDuration);
        
        if (_enemy.State == Enemy.EnemyState.Dead) yield break;

        _enemy.ChangeState(Enemy.EnemyState.StartSeeking);
    }
}

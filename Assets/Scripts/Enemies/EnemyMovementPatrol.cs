using System.Collections;
using UnityEngine;

public class EnemyMovementPatrol : MonoBehaviour
{
    [SerializeField] private float moveForce;
    [SerializeField] private float maxVelocity;

    [SerializeField] private float sleepingTime;

    [SerializeField] private ScoreBalloon scoreBalloonPrefab;

    private Rigidbody2D _body;
    private Animator _animator;
    private Enemy _enemy;

    private bool _isInRotationPhase;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }
    
    void Update()
    {
        switch (_enemy.State)
        {
            case Enemy.EnemyState.Dead:
                return;
            case Enemy.EnemyState.Walking:
            {
                float direction = _enemy.IsFacingRight ? 1f : -1f;
            
                _body.AddForce(new Vector2(moveForce * Time.deltaTime * direction, 0));
            
                _body.velocity = new Vector2(Mathf.Clamp(_body.velocity.x, -maxVelocity, maxVelocity), _body.velocity.y);

                if (_enemy.IsOnRotatePosition && !_isInRotationPhase)
                {
                    _isInRotationPhase = true;
                    _enemy.IsFacingRight = !_enemy.IsFacingRight;
                    _enemy.ChangeState(Enemy.EnemyState.Sleeping);

                    StartCoroutine(WaitAndMove());
                }
                else if (_isInRotationPhase && !_enemy.IsOnRotatePosition)
                {
                    _isInRotationPhase = false;
                }
                
                break;
            }
            case Enemy.EnemyState.Hit:
            {
                float direction = transform.position.x - _enemy.CurrentPlayerPosition.x > 0 ? 1f : -1f;
            
                _body.AddForce(new Vector2(direction * 150f, 150f));
                
                GameState.Score += 10;
                
                var scoreBalloon = Instantiate(scoreBalloonPrefab, transform.position, Quaternion.identity);
                scoreBalloon.SetScore(10);
            
                _enemy.ChangeState(Enemy.EnemyState.Dead);

                break;
            }
            default:
                // stop moving to side but allow falling
                _body.velocity = new Vector2(0, _body.velocity.y);
                break;
        }

        _animator.SetFloat("VelocityX", Mathf.Abs(_body.velocity.x));
    }

    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(sleepingTime);

        if (_enemy.State == Enemy.EnemyState.Dead) yield break;

        _enemy.ChangeState(Enemy.EnemyState.Walking);
    }
}

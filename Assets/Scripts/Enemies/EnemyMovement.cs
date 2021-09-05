using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveForce;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float jumpForce;

    private Rigidbody2D _body;
    private Animator _animator;
    private Enemy _enemy;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemy.State)
        {
            case Enemy.EnemyState.Dead:
                return;
            case Enemy.EnemyState.Walking:
            {
                float distance = _enemy.LastPlayerPosition.x - transform.position.x;

                if (Mathf.Abs(distance) < 0.02f)
                {
                    _enemy.ChangeState(Enemy.EnemyState.StartSeeking);
                }

                float direction = distance > 0 ? 1f : -1f;
            
                _body.AddForce(new Vector2(moveForce * Time.deltaTime * direction, 0));
                _body.velocity = new Vector2(Mathf.Clamp(_body.velocity.x, -maxVelocity, maxVelocity), _body.velocity.y);

                _enemy.IsFacingRight = direction > 0;
                
                break;
            }
            case Enemy.EnemyState.Hit:
            {
                float direction = transform.position.x - _enemy.CurrentPlayerPosition.x > 0 ? 1f : -1f;
            
                _body.AddForce(new Vector2(direction * 150f, 150f));
            
                _enemy.ChangeState(Enemy.EnemyState.Dead);

                GameState.Score += 10;
                
                break;
            }
            default:
                // stop moving to side but allow falling
                _body.velocity = new Vector2(0, _body.velocity.y);
                break;
        }

        _animator.SetFloat("VelocityX", Mathf.Abs(_body.velocity.x));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_enemy.State == Enemy.EnemyState.Dead)
            return;
        
        LadderEdge edge = other.GetComponent<LadderEdge>();
        if (edge)
        {
            float distance = _enemy.LastPlayerPosition.y - transform.position.y;

            if (distance >= 0)
            {
                // jump to other side of ledder
                _body.AddForce(new Vector2(0, jumpForce));
            }
        }
    }
}

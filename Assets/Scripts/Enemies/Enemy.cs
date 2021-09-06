using UnityEngine;

public class Enemy : MonoBehaviour, IDeadZone
{
    public enum EnemyState
    {
        Idle,
        Spawned,
        StartSeeking,
        FinishSeeking,
        Walking,
        Sleeping,
        Attack,
        Hit,
        Dead
    }
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;

    private Animator _animator;
    private CircleCollider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    
    private Player _player;
    private EnemyState _state;
    private Vector2 _lastPlayerPosition;
    private bool _isGrounded;

    public bool IsOnRotatePosition { get; private set; }
    public bool IsFacingRight { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void OnLevelFinished()
    {
        ChangeState(EnemyState.Dead);
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        
        ChangeState(EnemyState.Spawned);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void Update()
    {
        if (State == EnemyState.Dead)
            return;
        
        if (_state == EnemyState.FinishSeeking)
        {
            _lastPlayerPosition = _player.transform.position;
            
            ChangeState(EnemyState.Walking);
        }

        Vector3 start = transform.position;
        Vector3 end = groundCheck.position;
        
        start.x -= 0.05f;
        end.x -= 0.05f;
        RaycastHit2D hitLeft = Physics2D.Linecast(start, end, groundLayerMask);
        Debug.DrawLine(start, end, Color.red);
        
        start.x += 0.1f;
        end.x += 0.1f;
        RaycastHit2D hitRight = Physics2D.Linecast(start, end, groundLayerMask);
        Debug.DrawLine(start, end, Color.red);

        IsGrounded = hitLeft.collider || hitRight.collider;
        
        start = transform.position;
        end = groundCheck.position;
        
        start.x -= 0.2f;
        end.x -= 0.2f;
        RaycastHit2D rotationHitLeft = Physics2D.Linecast(start, end, groundLayerMask);
        Debug.DrawLine(start, end, Color.green);
        
        start.x += 0.4f;
        end.x += 0.4f;
        RaycastHit2D rotationHitRight = Physics2D.Linecast(start, end, groundLayerMask);
        Debug.DrawLine(start, end, Color.green);

        IsOnRotatePosition = IsGrounded && !(rotationHitLeft.collider && rotationHitRight.collider);

        Vector3 spriteScale = _spriteRenderer.transform.localScale;
        spriteScale.x = IsFacingRight ? 1 : -1;
        _spriteRenderer.transform.localScale = spriteScale;
    }

    public void ChangeState(EnemyState state)
    {
        if (_state == EnemyState.Dead)
            return;
        
        _state = state;
        
        _animator.SetBool("IsSpawned", _state == EnemyState.Spawned);
        _animator.SetBool("IsSeeking", _state == EnemyState.StartSeeking);
        _animator.SetBool("IsUnderAttack", _state == EnemyState.Hit);
        _animator.SetBool("IsDead", _state == EnemyState.Dead);
        _animator.SetBool("IsAttacking", _state == EnemyState.Attack);

        if (_state == EnemyState.Dead)
        {
            _collider2D.enabled = false;
            
            EventManager.TriggerEvent(Events.ENEMY_DIED);
            
            Destroy(gameObject, 2f);
        }
    }

    public EnemyState State => _state;
    
    public Vector2 LastPlayerPosition => _lastPlayerPosition;
    
    public  Vector2 CurrentPlayerPosition => _player.transform.position;
    
    public bool IsGrounded
    {
        get => _isGrounded;
        
        set
        {
            _isGrounded = value;
            
            _animator.SetBool("IsGrounded", _isGrounded);
        }
    }
    
    public void OnDeadZone()
    {
        ChangeState(EnemyState.Dead);
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDeadZone
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayerMask;

    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private bool _isOnLadder;
    private bool _isGrounded;
    private bool _doAttack;
    private bool _isUnderAttack;
    private bool _isFacingRight = true;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventManager.TriggerEvent(Events.CAMERA_START_FOLLOWING);
        EventManager.AddListener(Events.TIME_OUT, OnTimeOut);
    }

    private void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = groundCheck.position;

        start.x += 0.15f;
        end.x += 0.15f;
        RaycastHit2D hitLeft = Physics2D.Linecast(transform.position, end, groundLayerMask);
        Debug.DrawLine(transform.position, end, Color.green);
        
        start.x -= 0.30f;
        end.x -= 0.30f;
        RaycastHit2D hitRight = Physics2D.Linecast(transform.position, end, groundLayerMask);
        Debug.DrawLine(transform.position, end, Color.green);

        IsGrounded = hitLeft.collider || hitRight.collider;
        
        Vector3 scale = _spriteRenderer.transform.localScale;
        scale.x = IsFacingRight ? 1 : -1;
    
        _spriteRenderer.transform.localScale = scale;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TIME_OUT, OnTimeOut);
    }

    public bool IsGrounded
    {
        get => _isGrounded;
        
        set
        {
            _isGrounded = value;
            
            _animator.SetBool("IsGrounded", _isGrounded);
        }
    }

    public bool IsUnderAttack
    {
        get => _isUnderAttack;
        
        set
        {
            _isUnderAttack = value;
            
            _animator.SetBool("IsUnderAttack", _isUnderAttack);
        }
    }

    public bool DoAttack
    {
        get => _doAttack;

        set
        {
            _doAttack = value;
            
            _animator.SetBool("IsAttacking", _doAttack);
        }
    }

    public bool IsDead { get; set; }

    public bool IsOnLadder
    {
        get => _isOnLadder;
        
        set
        {
            if (value == _isOnLadder)
                return;

            _isOnLadder = value;
            
            if (_isOnLadder)
            {
                _body.bodyType = RigidbodyType2D.Kinematic;
                _body.velocity = Vector2.zero;
            }
            else
            {
                _body.bodyType = RigidbodyType2D.Dynamic;
                
                // reset after climb
                _spriteRenderer.flipX = false;
            }
            
            _animator.SetBool("IsOnLadder", _isOnLadder);
        }
    }

    public bool IsCloseToLadder { get; set; }

    public bool IsFacingRight
    {
        get
        {
            if (!_isUnderAttack)
            {
                if (Mathf.Abs(_body.velocity.x) > 0.01f)
                    _isFacingRight = _body.velocity.x > 0;                
            }
            
            return _isFacingRight;
        }
    }

    public Vector2 Position => transform.position;
    
    private void OnTimeOut()
    {
        IsDead = true;

        StartCoroutine(PlayerIsDead());
    }
    
    public void OnDeadZone()
    {
        if (IsDead)
            return;
        
        IsDead = true;

        StartCoroutine(PlayerIsDead());
    }

    private IEnumerator PlayerIsDead()
    {
        EventManager.TriggerEvent(Events.PLAYER_DIED);
        
        _body.velocity = Vector2.zero;
        _body.AddForce(Vector2.up * 300f);
        
        yield return new WaitForSeconds(2.5f);

        GameState.Lives--;

        EventManager.TriggerEvent(Events.LEVEL_FAILED);
    }
}

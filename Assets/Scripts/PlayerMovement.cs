using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float ledgerClimbSpeed;
    
    private Rigidbody2D _body;
    private Player _player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isJumpRequested;
    private bool _isMovementEnabled;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _isMovementEnabled = true;
        
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnHitPlayer);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_UNDER_ATTACK, OnHitPlayer);
        EventManager.RemoveListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsDead)  return;
        
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CanJump())
        {
            _isJumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (_player.IsDead) return;
        if (_player.IsUnderAttack) return;
        if (!_isMovementEnabled) return;
        
        float horizontal = _player.DoAttack ? 0 : Input.GetAxis("Horizontal");
        float vertical = _player.DoAttack ? 0: Input.GetAxis("Vertical");

        if (!_player.IsOnLadder && _player.IsCloseToLadder && (
                Mathf.Abs(_body.velocity.y) > 0.05f || Mathf.Abs(vertical) > 0
            )
        )
        {
            _player.IsOnLadder = true;
        }
        
        if (_player.IsOnLadder)
        {
            transform.Translate(
                new Vector2(horizontal * Time.deltaTime, vertical * Time.deltaTime) * ledgerClimbSpeed
                );
            
            // player moves down and reach the platform
            if (_player.IsGrounded && vertical < 0)
                _player.IsOnLadder = false;
        }
        else
        {
            Vector2 moveForce = new Vector2(runSpeed * Time.deltaTime * horizontal, 0);

            if (_isJumpRequested)
            {
                moveForce.y = jumpForce;
                _isJumpRequested = false;
                
                EventManager.TriggerEvent(Events.PLAYER_JUMP);
            }

            _body.AddForce(moveForce);

            float currentMaxVelocity = Mathf.Abs(horizontal) > 0 ? maxVelocity : 0; 

            _body.velocity = new Vector2(
                Mathf.Clamp(_body.velocity.x, -currentMaxVelocity, currentMaxVelocity), 
                _body.velocity.y
            );
        }
        
        _animator.SetBool("IsClimbing", _player.IsOnLadder && (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0.5f));
        _animator.SetBool("IsOnLadder", _player.IsOnLadder);
        _animator.SetBool("IsRunning", !_player.IsOnLadder && Mathf.Abs(_body.velocity.x) > 0);
    }

    bool CanJump()
    {
        return _player.IsGrounded && !_player.IsOnLadder;
    }
    
    private void OnLevelFinished()
    {
        _isMovementEnabled = false;
        
        _body.velocity = Vector2.zero;
        _animator.SetBool("IsRunning", false);
    }

    void OnHitPlayer(Vector3 attackForce)
    {
        if (_player.IsUnderAttack)
            return;

        _player.IsUnderAttack = true;
        _player.IsOnLadder = false;
        
        _body.AddForce(attackForce);

        StartCoroutine(RecoverAfterHit());
    }

    IEnumerator RecoverAfterHit()
    {
        yield return new WaitForSeconds(0.5f);

        _player.IsUnderAttack = false;
    }

    public void PlayerClimb()
    {
        // change facing during climb
        if (_player.IsOnLadder)
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
}

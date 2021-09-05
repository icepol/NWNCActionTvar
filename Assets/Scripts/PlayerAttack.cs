using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerRope rope;
    
    private Player _player;
    private bool _canAttack;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        rope.Hide();

        _canAttack = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_player.IsOnLadder)
            StartAttack();
    }

    private void StartAttack()
    {
        if (!_canAttack)
            return;
        
        if (!_player.IsGrounded)
            return;

        _canAttack = false;
        _player.DoAttack = true;
        
        rope.Show();

        StartCoroutine(FinishAttack());
    }

    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(0.5f);
        
        rope.Hide();
        
        _player.DoAttack = false;
        _canAttack = true;
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Steak : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    private Animator _animator;
    private bool _isAvailable = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        arrow.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player _player = other.GetComponent<Player>();
        if (_player && _isAvailable)
        {
            _isAvailable = false;
            
            arrow.SetActive(false);
            
            _animator.SetTrigger("Take");
            
            EventManager.TriggerEvent(Events.TAKE_STAKE);
            
            GameState.Score += 25;
            
            Destroy(gameObject,1);
        }
    }
}

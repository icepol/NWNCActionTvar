using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.RemoveListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.RemoveListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnPlayerShoot()
    {
        _animator.SetTrigger("ShakeSmall");
    }

    private void OnPlayerUnderAttack(Vector3 _)
    {
        _animator.SetTrigger("ShakeSmall");
    }

    void OnPlayerDied()
    {
        _animator.SetTrigger("ShakeBig");
    }

    void OnEnemyDied()
    {
        _animator.SetTrigger("ShakeSmall");
    }
}

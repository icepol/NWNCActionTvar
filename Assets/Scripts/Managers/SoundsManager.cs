using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private AudioClip playerAttack;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private AudioClip playerDie;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip spawnEnemy;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip takeSteak;
    [SerializeField] private AudioClip findFire;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.AddListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.AddListener(Events.ENEMY_SPAWNED, OnEnemySpawned);
        EventManager.AddListener(Events.ENEMY_UNDER_ATTACK, OnEnemyUnderAttack);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.AddListener(Events.TAKE_STAKE, OnTakeStake);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void OnDestroy()
    {
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.AddListener(Events.PLAYER_UNDER_ATTACK, OnPlayerUnderAttack);
        EventManager.AddListener(Events.PLAYER_JUMP, OnPlayerJump);
        EventManager.AddListener(Events.ENEMY_SPAWNED, OnEnemySpawned);
        EventManager.AddListener(Events.ENEMY_UNDER_ATTACK, OnEnemyUnderAttack);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
        EventManager.AddListener(Events.TAKE_STAKE, OnTakeStake);
        EventManager.AddListener(Events.LEVEL_FINISHED, OnLevelFinished);
    }

    private void OnPlayerUnderAttack(Vector3 _)
    {
        if (playerHit && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(playerHit, _cameraTransform.position);
    }

    private void OnPlayerJump()
    {
        if (playerJump && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(playerJump, _cameraTransform.position);
    }

    private void OnEnemySpawned()
    {
        if (spawnEnemy && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(spawnEnemy, _cameraTransform.position);
    }

    private void OnEnemyUnderAttack()
    {
        if (enemyHit && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(enemyHit, _cameraTransform.position);
    }

    private void OnTakeStake()
    {
        if (takeSteak && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(takeSteak, _cameraTransform.position);
    }

    private void OnLevelFinished()
    {
        if (findFire && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(findFire, _cameraTransform.position);
    }

    private void OnPlayerDied()
    {
        if (playerDie && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(playerDie, _cameraTransform.position);
    }

    private void OnPlayerShoot()
    {
        if (playerAttack && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(playerAttack, _cameraTransform.position);
    }

    private void OnEnemyDied()
    {
        if (enemyDie && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(enemyDie, _cameraTransform.position);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private AudioClip fire;
    [SerializeField] private AudioClip playerDie;
    [SerializeField] private AudioClip toolCollected;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        
        EventManager.AddListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.AddListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.AddListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.PLAYER_DIED, OnPlayerDied);
        EventManager.RemoveListener(Events.PLAYER_SHOOT, OnPlayerShoot);
        EventManager.RemoveListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    public void OnEnemyDied()
    {
        if (enemyDie && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(enemyDie, _cameraTransform.position);
    }

    public void OnPlayerShoot()
    {
        if (fire && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(fire, _cameraTransform.position);
    }

    public void OnPlayerDied()
    {
        if (playerDie && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(playerDie, _cameraTransform.position);
    }
    
    public void OnToolCollected()
    {
        if (toolCollected && Settings.IsSfxEnabled)
            AudioSource.PlayClipAtPoint(toolCollected, _cameraTransform.position);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject boss;

    [Header("Spawning before steak pickup")]
    [SerializeField] private float firstSpawnDelay;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    
    [Header("Spawning after steak pickup")]
    [SerializeField] private float minSpawnDelayAfterPickup;
    [SerializeField] private float maxSpawnDelayAfterPickup;

    private float _minSpawnDelay;
    private float _maxSpawnDelay;
    
    void Start()
    {
        _minSpawnDelay = minSpawnDelay;
        _maxSpawnDelay = maxSpawnDelay;
        
        EventManager.AddListener(Events.TAKE_STAKE, OnTakeSteak);
        
        StartCoroutine(Spawn());
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.TAKE_STAKE, OnTakeSteak);
    }

    private void OnTakeSteak()
    {
        _minSpawnDelay = minSpawnDelayAfterPickup;
        _maxSpawnDelay = maxSpawnDelayAfterPickup;

        if (boss)
            Instantiate(boss, transform.position, Quaternion.identity, transform);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(firstSpawnDelay);
        
        while (true)
        {
            GameObject spawned = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position,
                Quaternion.identity, transform);
            
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
    }
}

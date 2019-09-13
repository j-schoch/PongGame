using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnables;
    [SerializeField] private float _initialDelay;
    [SerializeField] private float _minTimeBetweenSpawns;
    [SerializeField] private Vector2 _spawnDelayRange;
    [SerializeField] private BoxCollider2D _spawnArea;
    [SerializeField] private float _maxSpawnLifetime;
    [SerializeField] private int _maxSpawnsExisting;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private float _nextSpawnTime;
    private float _timeSinceLastSpawn;

    private void OnEnable()
    {
        ScheduleNextSpawn(_initialDelay);
        _timeSinceLastSpawn = 0;
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if(_timeSinceLastSpawn >= _nextSpawnTime && _spawnedObjects.Count < _maxSpawnsExisting)
        {
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        _timeSinceLastSpawn = 0;
        ScheduleNextSpawn(Random.Range(_spawnDelayRange.x, _spawnDelayRange.y));

        int randomIndex = Random.Range(0, _spawnables.Count);
        Vector2 position = _spawnArea != null ? RandomPointInBox(_spawnArea.bounds.center, _spawnArea.size) : (Vector2)transform.position;
        GameObject spawnedObject = Instantiate(_spawnables[randomIndex], position, Quaternion.identity);
        _spawnedObjects.Add(spawnedObject);
        StartCoroutine(KillAfterTime(spawnedObject, _maxSpawnLifetime));
    }

    private IEnumerator KillAfterTime(GameObject toKill, float time)
    {
        yield return new WaitForSeconds(time);
        if(toKill != null)
        {
            _spawnedObjects.Remove(toKill);
            Destroy(toKill);
        }
        else
        {
            _spawnedObjects.RemoveAll(obj => obj == null);
        }
    }

    private void ScheduleNextSpawn(float delay)
    {
        _nextSpawnTime = _minTimeBetweenSpawns + delay;
    }

    private Vector2 RandomPointInBox(Vector2 center, Vector2 size) 
    {
        return center + new Vector2(
            (Random.value - 0.5f) * size.x,
            (Random.value - 0.5f) * size.y
        );
    }
}

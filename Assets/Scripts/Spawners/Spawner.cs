using System;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform _objectsParent;

    protected Pool _pool;
    protected ISpawnPoint _spawnPoint;

    public Transform SpawnPoint => _spawnPoint.GetPoint();

    public void StartSpawn(int objectsCount)
    {
        _pool = GetComponentInChildren<Pool>();
        _spawnPoint = GetComponentInChildren<ISpawnPoint>();

        if (_pool == null)
        {
            throw new NullReferenceException(nameof(Pool));
        }

        if (_spawnPoint == null)
        {
            throw new NullReferenceException(nameof(ISpawnPoint));
        }

        if (_objectsParent == null)
        {
            throw new NullReferenceException(nameof(_objectsParent));
        }

        _pool.Initialize();

        CreatingObjects(objectsCount);
    }

    protected abstract void CreatingObjects(int objectsCount);
}

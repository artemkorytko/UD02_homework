using System;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    protected Transform _objectsParent;
    protected Pool _pool;
    protected ISpawnPoint _spawnPoint;

    private void Awake()
    {
        _objectsParent = GetComponentInChildren<ObjectsParentComponent>(true).transform;
        _pool = GetComponentInChildren<Pool>(true);
        _spawnPoint = GetComponentInChildren<ISpawnPoint>(true);

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
            throw new NullReferenceException(nameof(ObjectsParentComponent));
        }
    }
    
    public void StartSpawn(int objectsCount)
    {
        _pool.Initialize();

        CreatingObjects(objectsCount);
    }

    protected abstract void CreatingObjects(int objectsCount);
}

using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform _objectsParent;

    protected Pool _pool;
    protected ISpawnPoint _spawnPoint;

    public async UniTask StartSpawn(int objectsCount)
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

        await CreatingObjects(objectsCount);
    }

    protected abstract UniTask CreatingObjects(int objectsCount);
}

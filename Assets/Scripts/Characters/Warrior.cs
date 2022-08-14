using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Movable))]
public abstract class Warrior : MonoBehaviour, IPoolInitializable
{
    protected Movable _movable;
    private Pool _pool;

    private void Awake()
    {
        _movable = GetComponent<Movable>();
    }

    public void PoolInitialize(Pool pool)
    {
        _pool = pool;
    }

    public async UniTask StartMove(Vector3 point)
    {
        if (_pool == null)
        {
            throw new Exception(nameof(Pool) + "is not initialized");
        }

        await _movable.Move(point);
    }

    public void ReturnToPool()
    {
        _pool.ReturnObject(gameObject);
    }
}

using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Movable))]
public abstract class Warrior : MonoBehaviour
{
    protected Movable _movable;
    private Pool _pool;

    public void Initialize(Pool pool)
    {
        _pool = pool;
        _movable = GetComponent<Movable>();
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

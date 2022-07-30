using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Movable))]
public class Warrior : MonoBehaviour
{
    private Movable _movable;
    private Pool _pool;
    private bool _isRewarded = false;

    public Action<Viking, Reward> OnRewarded;
    public bool IsStacked = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy) && !GetComponent<Enemy>())
        {
            enemy.ReturnToPool();
        }

        if (other.gameObject.TryGetComponent(out Reward reward) && _isRewarded == false && _movable.IsMoving)
        {
            _isRewarded = true;
            reward.gameObject.transform.SetParent(this.gameObject.transform);
            reward.gameObject.GetComponent<Collider>().isTrigger = false;
            OnRewarded?.Invoke(this as Viking, reward);
        }
    }

    public void ReturnToPool()
    {
        _pool.ReturnObject(this.gameObject);
    }
}

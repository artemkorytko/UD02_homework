using System;
using UnityEngine;

public class Viking : Warrior
{
    private bool _isRewarded = false;

    public Action<Viking, Reward> OnRewarded;
    public bool IsStacked = false;
    
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
}

using System;
using UnityEngine;

public class Viking : Warrior
{
    public Action<Viking, RewardComponent> OnRewarded;
    public bool IsStacked;
    
    private bool _isRewarded;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ReturnToPool();
        }
        else if (other.gameObject.TryGetComponent(out RewardComponent reward) && _isRewarded == false)
        {
            _isRewarded = true;
            reward.gameObject.transform.SetParent(gameObject.transform);
            reward.gameObject.GetComponent<Collider>().enabled = false;
            OnRewarded?.Invoke(this, reward);
        }
    }
}

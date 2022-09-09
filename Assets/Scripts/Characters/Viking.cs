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
            var rewardGameObject = reward.gameObject;
            Vector3 rewardPosition = GetComponentInChildren<RewardTransformComponent>().
                                       gameObject.transform.position;

            rewardGameObject.transform.SetParent(gameObject.transform);
            rewardGameObject.GetComponent<Collider>().enabled = false;
            rewardGameObject.transform.position = rewardPosition;
            rewardGameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            OnRewarded?.Invoke(this, reward);
            _isRewarded = true;
        }
    }
}

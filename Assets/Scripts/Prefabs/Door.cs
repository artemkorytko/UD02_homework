using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Stack<Viking> _vikingsStack = new Stack<Viking>();
    private List<RewardComponent> _rewards = new List<RewardComponent>();
    private int _vikingsCount;

    public void SetRewardsList(int count)
    {
        var doorPosition = transform.position;
        _vikingsCount = count;
        _rewards.AddRange(GetComponentsInChildren<RewardComponent>());

        if (_rewards.Count == 0)
        {
            throw new ArgumentNullException(nameof(_rewards.Count));
        }

        for (var i = 0; i < _vikingsCount - 1; i++)
        {
            for (var j = i + 1; j > _vikingsCount; j++)
            {
                if ((_rewards[i].gameObject.transform.position - doorPosition).sqrMagnitude >
                    (_rewards[j].gameObject.transform.position - doorPosition).sqrMagnitude)
                {
                    (_rewards[i], _rewards[j]) = (_rewards[j], _rewards[i]);
                }
            }
        }
    }

    public Vector3 GetPoint()
    {
        if (_rewards.Count > 0)
        {
            var point = _rewards[0].transform.position;
            _rewards.RemoveAt(0);
            return point;
        }
        else
        {
            throw new ArgumentNullException(nameof(_rewards.Count));
        }
    }
    
    public void GetPoint(RewardComponent rewardComponent)
    {
        if (_rewards.Count > 0)
        {
            foreach (var reward in _rewards)
            {
                if (rewardComponent.gameObject.transform.position == reward.transform.position)
                {
                    _rewards.Remove(reward);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Viking viking))
        {
            if (viking.IsStacked == false)
            {
                _vikingsStack.Push(viking);
                viking.IsStacked = true;
            }
        }
    }

    public Viking GetStack()
    {
        if (_vikingsStack.Count > 0) return _vikingsStack.Pop();
        else return null;
    }
}

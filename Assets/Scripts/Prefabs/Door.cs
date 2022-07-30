using System;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Stack<Viking> _vikingsStack = new Stack<Viking>();
    private List<Reward> _rewards = new List<Reward>();
    private int _vikingsCount;

    public void Initialize(int count)
    {
        var doorPosition = transform.position;
        _vikingsCount = count;
        _rewards.AddRange(GetComponentsInChildren<Reward>());

        for (int i = 0; i < _vikingsCount - 1; i++)
        {
            for (int j = i + 1; j > _vikingsCount; j++)
            {
                if ((_rewards[i].gameObject.transform.position - doorPosition).sqrMagnitude >
                    (_rewards[j].gameObject.transform.position - doorPosition).sqrMagnitude)
                {
                    var reward = _rewards[i];
                    _rewards[i] = _rewards[j];
                    _rewards[j] = reward;
                }
            }
        }
    }

    public Vector3 GetPoint()
    {
        if (_rewards.Count > 0)
        {
            Vector3 point = _rewards[0].transform.position;
            _rewards.RemoveAt(0);
            return point;
        }
        else
        {
            throw new ArgumentNullException(nameof(_rewards.Count));
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

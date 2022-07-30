using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class PointsSpawner : Spawner, IPoints
{
    [SerializeField] protected float _creatingWidth;
    [SerializeField] protected float _zMaxPositionOffset;

    protected List<Vector3> _points = new List<Vector3>();
    private Vector3 _currentPointPosition = Vector3.zero;
    private int _readCount = 0;

    protected override async UniTask CreatingObjects(int objectsCount)
    {
        float startPositionX = _spawnPoint.GetPoint().position.x - _creatingWidth / 2f;
        float xPosition = startPositionX;
        float xStep = _creatingWidth / (objectsCount - 1);
        float zPosition;

        for (int i = 0; i < objectsCount; i++)
        {
            zPosition = _spawnPoint.GetPoint().position.z + UnityEngine.Random.Range(-_zMaxPositionOffset, _zMaxPositionOffset);
            GameObject created = _pool.GetObject(_objectsParent.transform);
            _currentPointPosition.x = xPosition;
            _currentPointPosition.z = zPosition;
            created.transform.position = _currentPointPosition;
            _points.Add(created.transform.position);
            xPosition += xStep;

            await UniTask.Yield();
        }
    }

    public Vector3 ReadPoint()
    {
        if (_points.Count >= _readCount)
        {
            Vector3 point = _points[_readCount];
            _readCount++;
            return point;
        }
        else
        {
            throw new ArgumentNullException(nameof(_points.Count));
        }
    }

    public Vector3 GetPoint()
    {
        if (_points.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _points.Count);
            Vector3 point = _points[index];
            _points.RemoveAt(index);
            return point;
        }
        else
        {
            throw new ArgumentNullException(nameof(_points.Count));
        }
    }
}

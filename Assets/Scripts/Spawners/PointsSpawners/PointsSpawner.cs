using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PointsSpawner : Spawner, IPoints
{
    [SerializeField] protected float _creatingWidth;
    [SerializeField] protected float _zMaxPositionOffset;

    private readonly List<Transform> _points = new List<Transform>();
    private Vector3 _currentPointPosition = Vector3.zero;
    private int _readCount;

    protected override void CreatingObjects(int objectsCount)
    {
        var startPositionX = _spawnPoint.GetPoint().position.x - _creatingWidth / 2f;
        var xPosition = startPositionX;
        var xStep = _creatingWidth / (objectsCount - 1);

        for (var i = 0; i < objectsCount; i++)
        {
            var zPosition = _spawnPoint.GetPoint().position.z + UnityEngine.Random.Range(-_zMaxPositionOffset, _zMaxPositionOffset);
            var created = _pool.GetObject(_objectsParent.transform);
            _currentPointPosition.x = xPosition;
            _currentPointPosition.z = zPosition;
            created.transform.position = _currentPointPosition;
            _points.Add(created.transform);
            xPosition += xStep;
        }
    }

    public Vector3 ReadPoint()
    {
        if (_points.Count >= _readCount)
        {
            var point = _points[_readCount];
            _readCount++;
            return point.position;
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
            var index = UnityEngine.Random.Range(0, _points.Count);
            var point = _points[index];
            _points.RemoveAt(index);
            return point.position;
        }
        else
        {
            throw new ArgumentNullException(nameof(_points.Count));
        }
    }
}

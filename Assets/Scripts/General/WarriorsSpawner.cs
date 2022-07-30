using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class WarriorsSpawner : Spawner
{
    private IPoints _points = null;

    public void Initialize(IPoints points)
    {
        _points = points;
    }
    protected override async UniTask CreatingObjects(int objectsCount)
    {
        if (_points == null)
        {
            throw new Exception(nameof(IPoints) + "is not initialized");
        }

        for (int i = 0; i < objectsCount; i++)
        {
            GameObject created = _pool.GetObject(_objectsParent.transform);
            created.transform.position = _spawnPoint.GetPoint().position;

            if (created.TryGetComponent(out Viking viking))
            {
                viking.Initialize(_pool);
                await viking.StartMove(_points.GetPoint());
            }

            if (created.TryGetComponent(out Enemy enemy))
            {
                enemy.Initialize(_pool);
                await enemy.StartMove(_points.ReadPoint());
            }
        }
    }
}

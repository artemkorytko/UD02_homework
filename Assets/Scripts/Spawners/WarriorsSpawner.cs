using System;

public class WarriorsSpawner : Spawner
{
    private IPoints _startPoints = null;

    public void SetSpawnPoints(IPoints startPoints)
    {
        _startPoints = startPoints;
    }
    
    protected override void CreatingObjects(int objectsCount)
    {
        if (_startPoints == null)
        {
            throw new Exception(nameof(IPoints) + " is not initialized");
        }

        for (var i = 0; i < objectsCount; i++)
        {
            var created = _pool.GetObject(_objectsParent.transform);
            created.transform.position = _startPoints.ReadPoint();

            if (created.TryGetComponent(out IPoolInitializable pooled))
            {
                pooled.PoolInitialize(_pool);
            }
        }
    }
}

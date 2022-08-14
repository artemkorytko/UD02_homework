using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    public Transform GetPoint()
    {
        return gameObject.transform;
    }
}

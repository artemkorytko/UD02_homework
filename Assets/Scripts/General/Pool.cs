using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs = null;

    private Queue<GameObject> _objects = new Queue<GameObject>();
    private int _startCount = 10;
    private int _resizeCount = 5;

    public void Initialize()
    {
        AddingObjects(_startCount);
    }

    public GameObject GetObject(Transform parent)
    {
        if (_objects.Count == 0)
        {
            AddingObjects(_resizeCount);
        }

        GameObject received = _objects.Dequeue();
        received.transform.SetParent(parent);
        received.SetActive(true);

        return received;
    }

    public void ReturnObject(GameObject returned)
    {
        returned.SetActive(false);
        returned.transform.SetParent(this.gameObject.transform);
        _objects.Enqueue(returned);
    }

    private void AddingObjects(int size)
    {
        int prefabIndex = 0;

        for (int i = 0; i < size; i++)
        {
            if (_prefabs.Length > 1)
            {
                prefabIndex = UnityEngine.Random.Range(0, _prefabs.Length);
            }

            if (_prefabs[prefabIndex] == null)
            {
                throw new NullReferenceException();
            }

            GameObject prefab = Instantiate(_prefabs[prefabIndex], transform);
            prefab.SetActive(false);
            _objects.Enqueue(prefab);
        }
    }
}

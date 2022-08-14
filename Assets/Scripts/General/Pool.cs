using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs = null;

    private Queue<GameObject> _objects = new Queue<GameObject>();
    private const int START_COUNT = 10;
    private const int RESIZE_COUNT = 5;

    public void Initialize()
    {
        AddingObjects(START_COUNT);
    }

    public GameObject GetObject(Transform parent)
    {
        if (_objects.Count == 0)
        {
            AddingObjects(RESIZE_COUNT);
        }

        var givenOut = _objects.Dequeue();
        givenOut.transform.SetParent(parent);
        givenOut.SetActive(true);

        return givenOut;
    }

    public void ReturnObject(GameObject returned)
    {
        returned.SetActive(false);
        returned.transform.SetParent(gameObject.transform);
        _objects.Enqueue(returned);
    }

    private void AddingObjects(int size)
    {
        var prefabIndex = 0;

        for (var i = 0; i < size; i++)
        {
            if (_prefabs.Length > 1)
            {
                prefabIndex = UnityEngine.Random.Range(0, _prefabs.Length);
            }

            if (_prefabs[prefabIndex] == null)
            {
                throw new NullReferenceException();
            }

            var prefab = Instantiate(_prefabs[prefabIndex], transform);
            prefab.SetActive(false);
            _objects.Enqueue(prefab);
        }
    }
}

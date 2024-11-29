using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _initialPoolSize = 5;
    [SerializeField] private int _maxPoolSize = 10;

    private Queue<GameObject> _pool;
    private List<GameObject> _allObjects;

    private void Start()
    {
        _pool = new Queue<GameObject>();
        _allObjects = new List<GameObject>();

        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateObject();
        }
    }

    public GameObject GetObject()
    {
        GameObject obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            if (_allObjects.Count >= _maxPoolSize)
            {
                Destroy(_allObjects[0]);
                _allObjects.RemoveAt(0);
            }

            obj = CreateObject();
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(_objectPrefab);
        _allObjects.Add(obj);
        obj.SetActive(false);
        return obj;
    }
}

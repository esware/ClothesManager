using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Pool
{
    public int poolSize;
    public GameObject prefab;
    
}

public class ObjectPool : MonoBehaviour
{
    public Pool[] Pools;
    public GameObject prefab;
    public int poolSize = 10;

    public List<GameObject> objectPool;

    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPool>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("ObjectPoolSingleton");
                    instance = singleton.AddComponent<ObjectPool>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.name = i.ToString();
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(true);
                return objectPool[i];
            }
        }

        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.SetParent(null);
        obj.SetActive(false);
    }
}
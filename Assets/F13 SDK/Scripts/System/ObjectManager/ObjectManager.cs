using System.Collections.Generic;
using UnityEngine;
using Assets.F13SDK.Scripts;

public class ObjectManager : OmegaSingletonManager<ObjectManager>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public void AwakeObjectManager()
    {
        // Instance = this;
    }
    #endregion 
    public List<Pool> pools;
    private bool isActive;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public bool Active
    {
        get { return isActive; }
        set
        {
            isActive = value;
        }
    }

    public void StartObjectManager()
    {
        if (!isActive)
        {
            this.gameObject.SetActive(false);
            return;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void DestoryFromPool(string tag, GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
        }
        if (poolDictionary[tag].Contains(gameObject))
        {
            gameObject.SetActive(false);
        }
    }

    public void DeActivateAllObjectFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
        }
        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            GameObject tempObject = poolDictionary[tag].Dequeue();
            if (tempObject.activeInHierarchy)
            {
                tempObject.SetActive(false);
            }
            poolDictionary[tag].Enqueue(tempObject);
        }
            
    }

}

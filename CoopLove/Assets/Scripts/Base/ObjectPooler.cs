using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
            Instance = this;
        //If instance already exists and it's not this:
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        //GameObject poolParent = new GameObject("Pools");

        foreach (Pool pool in pools)
        {
            GameObject poolType = new GameObject(pool.tag);
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; ++i)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                //obj.transform.parent = poolType.transform;
                objectPool.Enqueue(obj);
                DontDestroyOnLoad(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            // poolType.transform.parent = poolParent.transform;

            DontDestroyOnLoad(poolType);
        }

        DontDestroyOnLoad(gameObject);
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"ObjectPooler: Pool with tag {tag} does not exist");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);

        return obj;
    }
}

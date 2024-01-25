using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPool;
        public int amountToPool;
        public Transform poolParent;
        public bool shouldExpand;
    }

    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler Instance { get; private set; }
        private ObjectPooler() { }

        public List<GameObject> pooledObjects;
        public List<ObjectPoolItem> itemsToPool;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            foreach (var item in itemsToPool)
            {
                for (var i = 0; i < item.amountToPool; i++)
                {
                    GameObject obj = Instantiate(item.objectToPool, item.poolParent);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(string tag)
        {
            foreach (var pooledObject in pooledObjects)
            {
                if (!pooledObject.activeInHierarchy && pooledObject.CompareTag(tag))
                {
                    return pooledObject;
                }
            }

            foreach (var item in itemsToPool)
            {
                if (item.objectToPool.CompareTag(tag))
                {
                    if (item.shouldExpand)
                    {
                        GameObject obj = Instantiate(item.objectToPool, item.poolParent);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                        return obj;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public void ReleaseObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}

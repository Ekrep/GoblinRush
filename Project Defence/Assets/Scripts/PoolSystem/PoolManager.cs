using System.Collections.Generic;
using UnityEngine;
using MyPoolable = PoolSystem.Poolable.IPoolable;

namespace PoolSystem
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;
        public static PoolManager Instance => instance;
        private Dictionary<string, Queue<MonoBehaviour>> _pools = new();
        private Dictionary<string, MonoBehaviour> _prefabPools = new();

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void CreatePool<T>(string poolName, T poolObject, int amount, Transform poolParentObject) where T : MonoBehaviour, MyPoolable
        {
            if (_pools.ContainsKey(poolName))
            {
                var createdObject = Instantiate(poolObject, poolParentObject);
                AssignItemToPool(poolName, createdObject);
            }
            else
            {
                Queue<T> pool = new();
                for (int i = 0; i < amount; i++)
                {
                    var createdObject = Instantiate(poolObject, poolParentObject);
                    pool.Enqueue(createdObject);
                    createdObject.OnCreatedForPool();
                    createdObject.OnAssignPool();
                }
                _pools[poolName] = new Queue<MonoBehaviour>(pool);
                _prefabPools[poolName] = poolObject;
            }
        }
        public bool TryCreatePool<T>(string poolName, T poolObject, int amount, Transform poolParentObject) where T : MonoBehaviour, MyPoolable
        {
            if (_pools.ContainsKey(poolName))
            {
                return false;
            }

            Queue<T> pool = new();
            for (int i = 0; i < amount; i++)
            {
                var createdObject = Instantiate(poolObject, poolParentObject);
                pool.Enqueue(createdObject);
                createdObject.OnCreatedForPool();
                createdObject.OnAssignPool();
            }

            _pools[poolName] = new Queue<MonoBehaviour>(pool);
            _prefabPools[poolName] = poolObject;
            return true;
        }

        public void RemovePool(string poolName)
        {
            _pools.Remove(poolName);
            _prefabPools.Remove(poolName);
        }

        public void DeletePool<T>(string poolName) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.ContainsKey(poolName)) return;

            foreach (var obj in _pools[poolName])
            {
                if (obj is T castedObj)
                {
                    castedObj.OnDeletePool();
                    Destroy(castedObj.gameObject);
                }
            }

            _pools.Remove(poolName);
            _prefabPools.Remove(poolName);
        }

        public Queue<T> GetPool<T>(string poolName) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.TryGetValue(poolName, out var existingPool))
            {
                Debug.LogError("Pool doesn't exist");
                return null;
            }

            Queue<T> typedPool = new();
            foreach (var obj in existingPool)
            {
                if (obj is T castedObj)
                    typedPool.Enqueue(castedObj);
                else
                    Debug.LogWarning($"Object in pool {poolName} cannot be cast to type {typeof(T)}");
            }
            return typedPool;
        }

        public void DebugPool()
        {
            foreach (var pool in _pools)
            {
                Debug.Log(pool.Key);
                Debug.Log(pool.Value.GetType());
                Debug.Log(pool.Value.Count + "Pool Count");
            }
        }
        public bool TryDequeueItemFromPool<T>(string poolName, out T item, Transform parentObjWhenInstantiated = null) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.TryGetValue(poolName, out var pool))
            {
                Debug.LogError("Pool doesn't exist");
                item = null;
                return false;
            }

            if (pool.TryDequeue(out var result) && result is T pooleable)
            {
                pooleable.OnDequeuePool();
                pooleable.transform.SetParent(parentObjWhenInstantiated);
                item = pooleable;
                return true;
            }
            else
            {
                var instantiatedObject = Instantiate(_prefabPools[poolName], parentObjWhenInstantiated);
                if (instantiatedObject is T newPooleable)
                {
                    newPooleable.OnCreatedForPool();//if it's bugged because of this!
                    newPooleable.OnDequeuePool();
                    item = newPooleable;
                    return true;
                }
                item = null;
                return false;
            }

        }

        public T DequeueItemFromPool<T>(string poolName, Transform parentObjWhenInstantiated = null) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.TryGetValue(poolName, out var pool))
            {
                Debug.LogError("Pool doesn't exist");
                return null;
            }

            if (pool.TryDequeue(out var result) && result is T pooleable)
            {
                pooleable.OnDequeuePool();
                pooleable.transform.SetParent(parentObjWhenInstantiated);
                return pooleable;
            }
            else
            {
                var instantiatedObject = Instantiate(_prefabPools[poolName], parentObjWhenInstantiated);
                if (instantiatedObject is T newPooleable)
                {
                    newPooleable.OnCreatedForPool();//if it's bugged because of this!
                    newPooleable.OnDequeuePool();
                    return newPooleable;
                }
                return null;
            }
        }

        private void AssignItemToPool<T>(string poolName, T item) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.ContainsKey(poolName))
            {
                Debug.LogError("Pool doesn't exist");
                return;
            }

            item.OnAssignPool();
            _pools[poolName].Enqueue(item);
        }
        public void EnqueueItemToPool<T>(string poolName, T item) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.ContainsKey(poolName))
            {
                Debug.LogError("Pool doesn't exist");
                return;
            }
            item.OnEnqueuePool();
            _pools[poolName].Enqueue(item);

        }

        public T Peek<T>(string poolName) where T : MonoBehaviour, MyPoolable
        {
            if (!_pools.TryGetValue(poolName, out var pool) || pool.Count == 0)
            {
                Debug.LogError("Pool doesn't exist or is empty");
                return null;
            }

            if (pool.Peek() is T item)
                return item;

            Debug.LogWarning($"Object in pool {poolName} cannot be cast to type {typeof(T)}");
            return null;
        }
    }
}


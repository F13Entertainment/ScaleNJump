using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }

    [System.Serializable]
    public class PoolDictionary : UnitySerializedDictionary<Type, List<MonoBehaviour>> { }

    [System.Serializable]
    public class PoolGroup
    {
        public PoolDictionary activePool = new PoolDictionary();
        public PoolDictionary passivePool = new PoolDictionary();
    }
    
    [System.Serializable]
    public class PoolGroupDictionary : UnitySerializedDictionary<string, PoolGroup> { }
    public class PoolManager : Singleton<PoolManager>
    {
        public const string DEFAULT_GROUP = "DEFAULT";
        [SerializeField] private PoolGroupDictionary poolGroups=new PoolGroupDictionary();


        public T Instantiate<T>(GameObject prefab,string groupTag=DEFAULT_GROUP) where T: MonoBehaviour
        {
            var type = typeof(T);

            if (!poolGroups.ContainsKey(groupTag))
            {
                poolGroups.Add(groupTag,new PoolGroup());
            }

            var passivePool = poolGroups[groupTag].passivePool;
            
            if (!passivePool.ContainsKey(type))
            {
                passivePool.Add(type,new List<MonoBehaviour>());
            }

            if (passivePool[type].Any())
            {
                var ghostCheck = passivePool[type].First();
                if(!ghostCheck)
                    CleanGhostsInPools();
            }

            if (!passivePool[type].Any())
            {
                var component = Instantiate(prefab,transform).GetComponent<T>();
                passivePool[type].Add(component);
            }

            var activePool = poolGroups[groupTag].activePool;
            var mono = passivePool[type].First();
            if (!activePool.ContainsKey(type))
            {
                activePool.Add(type,new List<MonoBehaviour>());
            }
            passivePool[type].Remove(mono);
            if(!mono.gameObject.activeSelf) mono.gameObject.SetActive(true);
            activePool[type].Add(mono);
            return mono as T;
        }
    
        public void Destroy<T>(T poolObject,string groupTag=DEFAULT_GROUP) where T: MonoBehaviour
        {
            var type = typeof(T);
            if (poolGroups.ContainsKey(groupTag))
            {
                var activePool = poolGroups[groupTag].activePool;
                var passivePool = poolGroups[groupTag].passivePool;

                if (activePool.ContainsKey(type))
                {
                    var mono = poolObject as MonoBehaviour;
                    if (activePool[type].Contains(mono))
                    {
                        activePool[type].Remove(mono);
                        mono.gameObject.SetActive(false);
                        passivePool[type].Add(mono);
                        return;
                    }
                }
            }
            
            GameObject.Destroy(poolObject.gameObject);
            Debug.LogWarning("PoolManager.Destroy(): pool not contains poolObject. poolObject directly destroyed!");
        }
        
        public void DestroyAll<T>(string groupTag=DEFAULT_GROUP)
        {
            var type = typeof(T);
            if (poolGroups.ContainsKey(groupTag))
            {
                var activePool = poolGroups[groupTag].activePool;
                var passivePool = poolGroups[groupTag].passivePool;
                if (activePool.ContainsKey(type))
                {
                    var list = activePool[type].ToList();
                    foreach (var mono in list)
                    {
                        if (activePool[type].Contains(mono))
                        {
                            activePool[type].Remove(mono);
                            mono.gameObject.SetActive(false);
                            passivePool[type].Add(mono);
                        }
                    }

                    list.Clear();
                }
            }
        }

        public void PrePool<T>(GameObject prefab,int count,string groupTag=DEFAULT_GROUP,Transform parent=null , UnityAction<T,int> action=null) where T:MonoBehaviour
        {
        
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                var poolObject = Instantiate<T>(prefab,groupTag);
                if(parent!=null) poolObject.transform.SetParent(parent);
                if(action!=null) action.Invoke(poolObject.GetComponent<T>(),i);
                list.Add(poolObject);
            }
            for (var i = 0; i < count; i++)
            {
                Destroy<T>(list[i],groupTag);
            }
            list.Clear();
            list = null;
        }
    
        private void CleanGhostsInPools()
        {
            Debug.Log("PoolManager.CleanGhostsInPools: Cleaning pool STARTED!!");

            foreach (var pool in poolGroups.Values)
            {
                var activePool = pool.activePool;
                var passivePool = pool.passivePool;
                foreach (var keyValuePair in activePool)
                {
                    var objPool = keyValuePair.Value;
                    var willDelete=new List<MonoBehaviour>();
                    foreach (var poolObj in objPool)
                    {
                        if(!poolObj)
                            willDelete.Add(poolObj);
                    }
                    foreach (var deleteObj in willDelete)
                    {
                        objPool.Remove(deleteObj);
                        Debug.Log("PoolManager.CleanGhostsInPools: Cleaned in activePool");
                    }
                    willDelete.Clear();
                    willDelete = null;
                }
                foreach (var keyValuePair in passivePool)
                {
                    var objPool = keyValuePair.Value;
                    var willDelete=new List<MonoBehaviour>();
                    foreach (var poolObj in objPool)
                    {
                        if(!poolObj)
                            willDelete.Add(poolObj);
                    }
                    foreach (var deleteObj in willDelete)
                    {
                        objPool.Remove(deleteObj);
                        Debug.Log("PoolManager.CleanGhostsInPools: Cleaned in passivePool");
                    }
                    willDelete.Clear();
                    willDelete = null;
                }
            }

            
            Debug.Log("PoolManager.CleanGhostsInPools: Cleaning pool FINISHED!!");

        }
    }
}
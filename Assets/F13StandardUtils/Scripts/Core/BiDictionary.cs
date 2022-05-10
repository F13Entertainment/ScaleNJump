using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace F13StandardUtils.Scripts.Core
{
    [System.Serializable]
    public class BiDictionary<T1, T2> 
    {
        [SerializeField] private  SerializedDictionary<T1, T2> _first;
        [SerializeField] private  SerializedDictionary<T2, T1> _second;
        public BiDictionary()
        {
            _first = new SerializedDictionary<T1, T2>();
            _second = new SerializedDictionary<T2, T1>();
        }
        public void Add(T1 t1, T2 t2)
        {
            if(t1==null)
            {
                Debug.LogWarning("BiDictionary.Add(): t1 cannot be null");
                return;
            };
            if (t2 == null)
            {
                Debug.LogWarning("BiDictionary.Add(): t2 cannot be null");
                return;
            }
            _first.Add(t1, t2);
            _second.Add(t2, t1);
        }
        
        public void Remove(T1 t1)
        {
            if(t1==null)
            {
                Debug.LogWarning("BiDictionary.Remove(): t1 cannot be null");
                return;
            };
            var t2 = _first[t1];
            _first.Remove(t1);
            _second.Remove(t2);
        }
        public void Remove(T2 t2)
        {
            if (t2 == null)
            {
                Debug.LogWarning("BiDictionary.Remove(): t2 cannot be null");
                return;
            }
            var t1 = _second[t2];
            _first.Remove(t1);
            _second.Remove(t2);
        }
        public bool Contains(T1 t1)
        {
            return t1!=null && _first.ContainsKey(t1);
        }
        public bool Contains(T2 t2)
        {
            return t2!=null && _second.ContainsKey(t2);
        }

        public List<T1> GetFirstValues()
        {
            var list = _first.Keys.ToList();
            return list;
        }
        
        public List<T2> GetSecondValues()
        {
            var list = _second.Keys.ToList();
            return list;
        }

        public void Clear()
        {
            _first.Clear();
            _second.Clear();
        }
        public T1 this[T2 val]
        {
            get
            {
                if (Contains(val))
                {
                    return _second[val];
                }
                return default(T1);
            }
        }
        public T2 this[T1 val]
        {
            get
            {
                if (Contains(val))
                {
                    return _first[val];
                }
                return default(T2);
            }
        }
    }
}
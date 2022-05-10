using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public static class Utils
    {
        public static bool RandomBool() => UnityEngine.Random.Range(0, 2) == 0;
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T> ?? enumerable.ToList(); 
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static Vector3 RandomNormalizedVector(bool randX=true,bool randY=true,bool randZ=true)
        {
            var randVec = new Vector3(
                randX?UnityEngine.Random.Range(-1f, 1f):0f,
                randY?UnityEngine.Random.Range(-1f, 1f):0f,
                randZ?UnityEngine.Random.Range(-1f, 1f):0f);
            return randVec.normalized;
        }
        
    }
}
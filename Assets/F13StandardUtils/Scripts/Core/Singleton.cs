using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting) {
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            return _instance;
                        }
                
                    }
                    return _instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;
    }
}


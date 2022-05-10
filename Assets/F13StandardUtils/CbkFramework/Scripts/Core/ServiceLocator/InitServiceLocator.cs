using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator
{
    public class InitServiceLocator : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain()
        {
            var gameObject = new GameObject {name = "ServiceLocator"};
            gameObject.AddComponent<ServiceLocator>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
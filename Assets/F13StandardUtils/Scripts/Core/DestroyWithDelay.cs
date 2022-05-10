using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class DestroyWithDelay : MonoBehaviour
    {
        [ShowInInspector] private static float Delay = 1.5f;
        private void OnEnable()
        {
            Invoke(nameof(DestroyWithDelayProcess),Delay);
        }

        private void DestroyWithDelayProcess()
        {
            Destroy(this.gameObject);
        }
    }
}

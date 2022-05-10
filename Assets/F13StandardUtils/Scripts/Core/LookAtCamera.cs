using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Awake()
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        private void FixedUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}

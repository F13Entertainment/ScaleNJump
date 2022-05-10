using F13StandardUtils.Crowd.Scripts;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class RotateAroundTrigger : MonoBehaviour
    {
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private bool isLeft = false;
        [SerializeField] private PlayerType trigger;
        [SerializeField] private Transform pivot;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(trigger.ToString()))
            {
                MoveZ.Instance.RotateAround(isLeft,pivot);
                _triggerCollider.enabled = false;
            }
        }
    }
}

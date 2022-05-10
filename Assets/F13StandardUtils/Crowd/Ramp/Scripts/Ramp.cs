using F13StandardUtils.Crowd.Scripts;
using UnityEngine;

namespace F13StandardUtils.Crowd.Ramp.Scripts
{
    public class Ramp : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
    
        private float jumpPower = 450;
        
        private void OnTriggerEnter(Collider other)
        {
            var crowdMember = other.GetComponent<CrowdMember>();
            if (crowdMember)
            {
            
                crowdMember.Jump(jumpPower);
            }
        }
    
    
    }
}

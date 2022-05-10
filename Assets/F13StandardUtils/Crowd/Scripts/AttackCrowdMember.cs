using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    
    public class AttackCrowdMember:MonoBehaviour
    {
        [SerializeField] private PlayerType toAttack=PlayerType.Enemy;
        [SerializeField] private bool destroyAfterAttack = false;
        [SerializeField] private bool instant = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(toAttack.ToString()))
            {
                var crowdMember = other.gameObject.GetComponent<CrowdMember>();
                if (crowdMember)
                {
                    crowdMember.owner.Kill(crowdMember,instant);
                    if (destroyAfterAttack)
                    {
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}
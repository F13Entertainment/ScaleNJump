using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdMemberParticle : MonoBehaviour
    {
        [ShowInInspector] private static float Delay = 1.5f;
        public List<Color> colors;
        [SerializeField] private List<ParticleSystem> _particleSystems;
    

        public void Play(PlayerType playerType)
        {
            foreach (var _particleSystem in _particleSystems)
            {
                var particleSystemMain = _particleSystem.main;
                particleSystemMain.startColor = colors[(int) playerType];
                _particleSystem.Play();
            }
            Invoke(nameof(DestroyWithDelayProcess),Delay);
        }
    
        private void DestroyWithDelayProcess()
        {
            PoolManager.Instance.Destroy(this);
        }

    }
}

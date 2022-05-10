using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public class DrawParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        [SerializeField]  private Color color;
        [SerializeField] private float delay = 0.5f;

        public void Play(Color c)
        {
            color = c;
            var particleMain = _particle.main;
            particleMain.startColor = color;
            _particle.Play();
            Invoke(nameof(DestroyWithDelay),delay);
        }

        public void DestroyWithDelay()
        {
            PoolManager.Instance.Destroy(this);
        }
    }
}

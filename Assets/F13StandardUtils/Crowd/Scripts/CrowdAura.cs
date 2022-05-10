using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdAura : MonoBehaviour
    {
        [SerializeField] private float duration = .5f;

        private MeshRenderer _meshRenderer;
        private Vector3 _defaultScale;
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultScale = transform.localScale;
        }

        private Sequence seq;
        [Button]
        public void TriggerIncrementExplosion()
        {
            if(seq!=null) seq.Kill();

            ShowAura();
            seq = DOTween.Sequence();
            seq.Append(transform.DOScale(3f*_defaultScale, duration));
            seq.Join(_meshRenderer.material.DOFade(0, duration));
        }
        [Button]
        public void TriggerDecrementExplosion()
        {
            if(seq!=null) seq.Kill();
            ShowAura();
            seq = DOTween.Sequence();
            _meshRenderer.material.DOFade(1, 0f);
            seq.Append(transform.DOScale(0f, duration));
            seq.Join(_meshRenderer.material.DOFade(0, duration));
        }
        [Button]
    
        public void ShowAura()
        {
            transform.DOScale(1f*_defaultScale, 0f);
            _meshRenderer.material.DOFade(1f, 0f);



        }
        [Button]
        public void HideAura()
        {
            transform.DOScale(0f, 0f);
            _meshRenderer.material.DOFade(0f, 0f);

        }

    }
}

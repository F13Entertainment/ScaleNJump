using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Gate.BreakableGate.Scripts
{
    public class BreakableGate : MonoBehaviour
    {
        [SerializeField] private GameObject glassWhole;
        [SerializeField] private GameObject glassBroken;
        [SerializeField] private GameObject explosionForce;
        [SerializeField] private GameObject frame;


        private Renderer[] brokenChildRenderers;
        private Material[] glassBrokenMaterials;

        [SerializeField,ReadOnly] private bool _isBreaked;
        public bool IsBreaked => _isBreaked;
    
        [Button]
        public void BreakGlass()
        {
            if(_isBreaked) return;
            _isBreaked = true;
        
            explosionForce.SetActive(true);
            glassWhole.SetActive(false);
            glassBroken.SetActive(true);
            frame.SetActive(false);

            brokenChildRenderers = glassBroken.GetComponentsInChildren<Renderer> ();
            glassBrokenMaterials = new Material[brokenChildRenderers.Length];
        
            for (int i = 0; i < brokenChildRenderers.Length; i++) {
           
                glassBrokenMaterials[i] = brokenChildRenderers[i].GetComponent<Renderer>().material;
            }

            foreach (Material mat in glassBrokenMaterials)
            {
                mat.DOFade(0f, 2f).OnComplete(() => {glassBroken.SetActive(false);});
            }
        }
    }
}

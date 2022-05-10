using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class PopupCrowdCount : MonoBehaviour
    {
        public static float Duration = 1.5f;
        [SerializeField] private GameObject popup;

        [Button]
        public void SpawnPopupText(int increment)
        {
            var go = Instantiate(popup, new Vector3(0,5,0),popup.transform.rotation);
            var tmpText = go.GetComponent<TMP_Text>();
            tmpText.text = (increment > 0 ? "+" : "") + increment;
            tmpText.DOFade(0, Duration*0.66f);
            tmpText.transform.DOMove(new Vector3(0, 15, 0), Duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(go);
            });
        
        }
    }
}

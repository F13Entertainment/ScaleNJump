using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public class DrawPopup : MonoBehaviour
    {
        public static float Duration = 1.5f;
        [SerializeField] private GameObject popup;
        
        [Button]
        public void SpawnPopupText(string message,Color color)
        {
            var tmpText = Instantiate(popup, new Vector3(0,5,0),popup.transform.rotation).GetComponent<TMP_Text>();
            tmpText.transform.SetParent(Camera.main.transform);
            tmpText.transform.localScale=new Vector3(-1,1,1);
            tmpText.text = message;
            tmpText.color = color;
            tmpText.DOFade(0, Duration*0.66f);
            tmpText.transform.localPosition = Vector3.up * 0.4f + Vector3.forward*1f;
            tmpText.transform.DOLocalMove(Vector3.up * 0.6f + Vector3.forward * 1f, Duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(tmpText.gameObject);
            });

            tmpText.transform.LookAt(Camera.main.transform);
        }
    }
}

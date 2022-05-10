using DG.Tweening;
using TMPro;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdCountText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private CrowdManager owner;


        private Vector3 defaultPos;
        private void Awake()
        {
            defaultPos = transform.localPosition;
        }

        private int lastCount=-1;
        private void LateUpdate()
        {
            var count = owner.Count;
            if (lastCount != count)
            {
                text.text = count.ToString();
                lastCount = count;
                text.transform.parent.gameObject.SetActive(lastCount != 0);
                UpdatePosition();
            }

        }

        private void UpdatePosition()
        {
            var newPos = defaultPos + Vector3.up * owner.Count * .01f;
            transform.DOLocalMove(newPos,1f).SetDelay(owner.PullDelay);
        }
    }
}

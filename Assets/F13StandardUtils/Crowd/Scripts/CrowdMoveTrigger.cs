using System.Collections.Generic;
using DG.Tweening;
using F13StandardUtils.Crowd.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME.Scripts.Player
{
    [System.Serializable]
    public class CrowdMove
    {
        public Transform target;
        public bool randX = false;
        public bool randZ = false;

    }
    
    public class CrowdMoveTrigger : MonoBehaviour
    {
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private CrowdManager _moveCrowd;
        [SerializeField] private List<CrowdMove> _destinations=new List<CrowdMove>();
        [SerializeField] private float speed = 10f;
        [SerializeField] private PlayerType _triggerPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(_triggerPlayer.ToString()))
            {
                Move();
            }
        }

        [Button]
        private void Move()
        {
            _triggerCollider.enabled = false;
            var seq = DOTween.Sequence();
            for (var index = 0; index < _destinations.Count; index++)
            {
                var destination = _destinations[index];
                var duration = ((index-1>=0 ? _destinations[index-1].target.position: _moveCrowd.transform.position) - destination.target.transform.position).magnitude / speed;
                var localPos = _moveCrowd.transform.parent.InverseTransformPoint(destination.target.transform.position);

                seq.Append(_moveCrowd.transform.DOLocalMove(localPos, duration).SetEase(Ease.Linear)
                    .OnStart(() =>
                    {
                        _moveCrowd.isPulling = false;
                        _moveCrowd.memberList.ForEach(m =>
                        {
                            m.CrowdMemberAnimController.isMove = true;
                        });
                    })
                    .OnUpdate(() =>
                    {
                        var direction = (destination.target.transform.position - _moveCrowd.transform.position).normalized *
                                        100;
                        var lookAt = _moveCrowd.transform.position +
                                     (_moveCrowd.type == PlayerType.Player ? direction : -direction);
                        _moveCrowd.memberList.ForEach(m =>
                        {
                            m.transform.LookAt(lookAt);
                            m.destinationPos += CrowdUtils.RandomNormalizedVector(destination.randX,randY: false,destination.randZ) * Time.deltaTime * 14;
                        });
                    }).OnComplete(() =>
                    {
                        var direction = Vector3.back * 100;
                        var lookAt = _moveCrowd.transform.position +
                                     (_moveCrowd.type == PlayerType.Player ? direction : -direction);
                        _moveCrowd.memberList.ForEach(m =>
                        {
                            m.transform.LookAt(lookAt);
                            m.CrowdMemberAnimController.isMove = false;
                        });
                    }));
            }
        }
    }
}

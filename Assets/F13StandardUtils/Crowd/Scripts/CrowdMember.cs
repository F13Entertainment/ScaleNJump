using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdMember : MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(UpdatePlayerType))] private PlayerType _type;
        [SerializeField] private Rigidbody _rigid;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private List<CrowdMemberAnimController> _playerTypeObjects;
        public bool isDeath = false;
        public bool isPulling = true;
        
        [ReadOnly] public CrowdManager owner;
        [ReadOnly] private float lastUpdateTime;


        public CrowdMemberAnimController CrowdMemberAnimController => _playerTypeObjects[(int) _type];
        public PlayerType PlayerType => _type;

        private Vector3 defaultScale;
        private void Awake()
        {
            defaultScale = transform.localScale;
        }
        
        private void OnEnable()
        {
            SpawnEffect();
            Invoke(nameof(EnableTriggerCollider),CrowdManager.DefaultPullDelay);
            destinationPos=Vector3.zero;
            transform.localScale = defaultScale;
            transform.localRotation = Quaternion.identity;
            isDeath = false;
            isPulling = true;

        }

        public Vector3 destinationPos;
        
        private void FixedUpdate()
        {
            if(!isDeath && isPulling)
                transform.localPosition = Vector3.Lerp(transform.localPosition, destinationPos, Time.deltaTime * CrowdManager.PullPower);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other && other.CompareTag(_type.Opposite().ToString()))
            {
                owner.Kill(this);
                if (owner.Count == 0)
                {
                    owner.StartCoroutine(WaitForEndOfFrame(() =>
                    {
                        owner.PullCrowd();
                        var crowdMember = other.GetComponent<CrowdMember>();
                        crowdMember.owner.PullCrowd();
                    }));
                }
            }
        }

        private IEnumerator WaitForEndOfFrame(UnityAction action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }
        
        
        public void SetPlayerType(PlayerType playerType)
        {
            _type = playerType;
            UpdatePlayerType();
        }
        
        public void Death(bool isInstant,Action onEndAction)
        {
            isDeath = true;
            transform.SetParent(owner.transform.parent);
            _triggerCollider.enabled = false;
            if (isInstant)
            {
                onEndAction?.Invoke();
            }
            else
            {
                CrowdMemberAnimController.Death(() =>
                {
                    transform.DOScale(0, 0.3f).OnComplete(() =>
                    {
                        onEndAction?.Invoke();
                    });
                });
            }

        }

        

        
        private void SpawnEffect()
        {
            transform.DOScale(0.5f, 0);
            transform.DOScale(1f, 0.5f);
        }
        
        private void UpdatePlayerType()
        {
            gameObject.tag = _type.ToString();
            for (var i = 0; i < _playerTypeObjects.Count; i++)
            {
                _playerTypeObjects[i].gameObject.SetActive(i==(int)_type);
            }
        }
        
        private void EnableTriggerCollider()=>_triggerCollider.enabled = true;

        private bool isJumping = false;
        [Button]
        public void Jump(float jumpPower)
        {
            if(isJumping) return;
            StartCoroutine(JumpCoroutine(jumpPower));
        }

        private IEnumerator JumpCoroutine(float jumpPower)
        {
            isJumping = true;
            isPulling = false;
            _rigid.isKinematic = false;
            _rigid.useGravity = true;
            _rigid.AddForce(Vector3.up * jumpPower);
            _triggerCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(()=>transform.localPosition.y<0.2f);
            isPulling = true;
            _rigid.isKinematic = true;
            _rigid.useGravity = false;
            _triggerCollider.enabled = true;
            isJumping = false;
        }
    }
}
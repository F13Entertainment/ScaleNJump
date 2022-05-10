using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class UnitAnimController : MonoBehaviour
    {
        public const string ANIM_IDLE = "isIdle";
        public const string ANIM_ATTACK = "isAttack";
        public const string ANIM_RUN = "isRun";
        public const string ANIM_DEATH = "isDeath";



        [SerializeField] protected Animator _animator;
        [SerializeField] protected List<Animator> _secondaryAnimatorList = new List<Animator>();
        [SerializeField, Range(0f, 1f)] protected float attackAnimActionNormalizedTime = 0.5f;

        protected string curentAnimState=string.Empty;

        public string CurentAnimState => curentAnimState;


        public void SetMirror(bool isMirror)
        {
            var scale = transform.localScale;
            scale.x *= isMirror ? -1 : 1;
            transform.localScale = scale;
        }

        [Button]
        public virtual void Idle()
        {
            TriggerAnimation(ANIM_IDLE);
        }
        
        [Button]
        public virtual void Run()
        {
            TriggerAnimation(ANIM_RUN);

        }

        [Button]
        public virtual void Attack(Action onMidAction = null, Action onEndAction = null)
        {
            TriggerAnimation(ANIM_ATTACK,onMidAction,onEndAction);
        }
        
        [Button]
        public virtual void Death(Action onMidAction = null, Action onEndAction = null)
        {
            TriggerAnimation(ANIM_DEATH,onMidAction,onEndAction);
        }

        public virtual void PauseAnimator()
        {
            SetSpeed(0);
        }

        public virtual void ResumeAnimator()
        {
            SetSpeed(1);
        }

        public virtual void SetSpeed(float x)
        {
            _animator.speed = x;
            foreach (var a in _secondaryAnimatorList)
            {
                a.speed = x;
            }
        }

        private IEnumerator ActionCoroutine(float midActionNormalizedTime, Action onMidAction = null, Action onEndAction = null)
        {
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= midActionNormalizedTime);
            onMidAction?.Invoke();
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            onEndAction?.Invoke();
        }
        
        [Button]
        public virtual void TriggerAnimation(string triggerKey,Action onMidAction = null, Action onEndAction = null)
        {
            if(CurentAnimState.Equals(triggerKey)) return;
            
            _animator.SetTrigger(triggerKey);
            if (onMidAction!=null || onEndAction !=null)
                StartCoroutine(ActionCoroutine(attackAnimActionNormalizedTime, onMidAction, onEndAction));
            foreach (var a in _secondaryAnimatorList)
            {
                a.SetTrigger(triggerKey);
            }
            curentAnimState = triggerKey;
        }
        
        [Button]
        public virtual void PlayAnimationWithName(string animName,float normalizedTime, bool pauseAnimator=false)
        {
            _animator.Play(animName,0,normalizedTime);
            foreach (var a in _secondaryAnimatorList)
            {
                a.Play(animName,0,normalizedTime);
            }
            curentAnimState = animName;
            SetSpeed(pauseAnimator?0:1);
        }
    }
}
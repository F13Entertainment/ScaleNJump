using System;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public class CrowdMemberAnimController : UnitAnimController
    {
        public PlayerType playerType;
        [SerializeField,ReadOnly] private Material _defaultMaterial;
        [SerializeField] private Material _deathMaterial;
        [SerializeField] private Renderer _renderer;
        

        public bool isMove = false;
        private bool lastIsMoving = true;
        public List<AnimatorOverrideController> animatorControllerList = new List<AnimatorOverrideController>();
        
        
        private void Awake()
        {
            // RandomMirror();
            RandomSpeed();
            _defaultMaterial = _renderer.sharedMaterial;

        }
        
        private void OnEnable()
        {
            curentAnimState=string.Empty;
            isMove = false;
            _renderer.sharedMaterial = _defaultMaterial;
            UpdateAnim();
        }

        
        private void LateUpdate()
        {
            if (MoveZ.Instance && playerType == PlayerType.Player)
            {
                isMove = MoveZ.Instance.isMove;
            }
            if (lastIsMoving != isMove)
            {
                UpdateAnim();
            }
        }

        private void UpdateAnim()
        {
            lastIsMoving = isMove;
            if (isMove)
                Run();
            else
                Idle();
        }


        private void RandomMirror()
        {
            var randomBool = Utils.RandomBool();
            SetMirror(randomBool);
        }
        private void RandomSpeed()
        {
            var randomSpeed = UnityEngine.Random.Range(0.85f, 1f);
            SetSpeed(randomSpeed);
        }
        
        public override void Death(Action onMidAction = null, Action onEndAction = null)
        {
            _animator.Rebind();
            _secondaryAnimatorList.ForEach(s=>s.Rebind());
            base.Death(onMidAction, onEndAction);
            _renderer.sharedMaterial = _deathMaterial;
        }
    
    }
}
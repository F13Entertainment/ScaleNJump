using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
    public class MoveZ : Singleton<MoveZ>
    {
        public bool isMove = true;
        private bool isRotating = false;
        [SerializeField] private float _velocityZ = -7.5f;
        [SerializeField] private List<Transform> moveList=new List<Transform>();
        [SerializeField] private float _multiplier = 1;
        [ShowInInspector] public float TrueSpeed => _velocityZ * _multiplier;
        public float VelocityZ => _velocityZ;
        public float Multiplier => _multiplier;

        [ShowInInspector] public bool IsRotating => isRotating;

        void LateUpdate()
        {
            if(isMove) moveList.ForEach(t=>t.position= t.position+(Vector3.forward*TrueSpeed*Time.deltaTime));
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
        
        public void SetVelocityZ(float velocityZ)
        {
            _velocityZ = velocityZ;
        }

        public void RestoreMultiplierDefault()
        {
            SetMultiplier(1);
        }
        
        [Button]
        public void RotateAround(bool isLeft, Transform rotatePivot)
        {
            var duration = 0.7f;
            foreach (var move in moveList)
            {
                var t = move;
                var angle = 0f;
                DOTween.To(() => angle, x =>
                {
                    var rotateAmount = Time.deltaTime * 90f/duration ;
                    if (90 - angle < rotateAmount)
                        rotateAmount = 90 - angle;
                    angle += rotateAmount;
                    t.RotateAround(rotatePivot.position,isLeft ? Vector3.up : Vector3.down, rotateAmount);
                }, 90, duration).OnStart(() =>
                {
                    isRotating = true;
                    isMove = false;
                }).OnComplete(() =>
                {
                    isRotating = false;
                    isMove = true;
                });
            }
        }
    
    }
}

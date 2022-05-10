using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Crowd.Scripts
{
    public enum PlayerType
    {
        Player=0,
        Enemy=1
    }

    public static class PlayerTypeExtensions
    {
        public static PlayerType Opposite(this PlayerType playerType)
        {
            var enumCount = Enum.GetValues(typeof(PlayerType)).Length;
            var val = ((int)playerType + 1) % enumCount;
            return (PlayerType) val;
        }
    }
    
    public enum FormationType
    {
        Circular,
        Rectangular
    }

    public class CrowdManager : MonoBehaviour
    {
        [ShowInInspector] public static float PullPower = 10f;
        [ShowInInspector] public static float DefaultPullDelay = .5f;
        [ShowInInspector] public static float circularInterval = 1.4f;
        [ShowInInspector] public static float circularRandomization = 0.3f;
        public float PullDelay=>(lastCount > Count) ? DefaultPullDelay : 0.01f;
    
        public PlayerType type;
        [SerializeField] private FormationType _formationType;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject particlePrefab;
        [ReadOnly] public List<CrowdMember> memberList = new List<CrowdMember>();
        public int initialCount;
        [ShowInInspector,ReadOnly] public int Count => memberList.Count;
        public FormationType FormationType => _formationType;
        [ReadOnly] public int lastIncrement;
        public bool isPulling = true;

        public CrowdAura crowdAura;
        public PopupCrowdCount popupCrowdCount;

        private void Awake()
        {
            SpawnInitial();
        }

        private void FixedUpdate()
        {
            PullProcess();
        }
        
        [Button]
        public void SetFormation(FormationType formationType)
        {
            _formationType = formationType;
            ForcePullCrowd();
        }
        
        [Button]
        public List<List<CrowdMember>> SplitCrowd(params float[] ratios)
        {
            var groups = new List<List<CrowdMember>>();
            for (var i = 0; i < ratios.Length; i++)
            {
                groups.Add(new List<CrowdMember>());
            }
            
            for (var i = 0; i < memberList.Count; i++)
            {
                var result = CrowdUtils.CalculateSplitGroupCircularPositionWithIndex(i,circularInterval,memberList.Count,ratios);
                memberList[i].destinationPos = result.pos;
                memberList[i].destinationPos+=CrowdUtils.RandomNormalizedVector(randY: false) * circularRandomization;
                groups[result.groupIndex].Add(memberList[i]);
            }
            return groups;
        }
    
        [Button]
        public void Spawn()
        {
            var crowdMember = PoolManager.Instance.Instantiate<CrowdMember>(prefab);
            crowdMember.enabled = true;
            crowdMember.transform.SetParent(transform);
            crowdMember.owner = this;
            crowdMember.SetPlayerType(type);
            crowdMember.transform.localPosition = CrowdUtils.RandomNormalizedVector()*circularRandomization;
            crowdMember.transform.localRotation = Quaternion.identity;
            memberList.Add(crowdMember);
            PullCrowd();
        }

    
        [Button]
        public void UpdateCount(int newValue)
        {
            var diff = newValue - Count;
            if (diff > 0)
            {
                Spawn(diff);
            }
            else
            {
                for (var i = 0; i < -diff; i++)
                {
                    if(Count>0)
                        Kill(Count-1);
                }
            }

            lastIncrement = diff;
        }
    
        [Button]
        public void Spawn(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Spawn();
            }

        }

        public void Kill(CrowdMember member,bool instant=true)
        {
            int index = memberList.IndexOf(member);
            if (index >= 0)
            {
                Kill(index,instant);
            }
        }
        [Button]
        public void Kill(int memberIndex, bool instant=true)
        {
            if (!memberList.Any())
            {
                Debug.LogWarning("There is no member in crowd");
                return;
            }
            memberIndex %= memberList.Count;
            PlayParticle(memberIndex);
            var member = memberList[memberIndex];
            member.Death(instant,()=>PoolManager.Instance.Destroy(member));
            memberList.RemoveAt(memberIndex);
            PullCrowd();
        }
        
        
    

        [ReadOnly] public float lastPullTime;
        [Button]
        public void PullCrowd()
        {
            lastPullTime = Time.time;
        }

        [SerializeField,ReadOnly] private int lastCount;
        private void PullProcess()
        {
            if (!isPulling) return;
            if(lastCount==Count) return;
            if((Time.time-lastPullTime) < PullDelay) return;
            lastCount = Count;
            ForcePullCrowd();
        }
        
        private void ForcePullCrowd()
        {
            for (var i = 0; i < memberList.Count; i++)
            {
                var pos = Vector3.zero;
                switch (_formationType)
                {
                    case FormationType.Circular:
                        pos = CrowdUtils.CalculateCircularPositionWithIndex(i, circularInterval);
                        pos += CrowdUtils.RandomNormalizedVector(randY: false) * circularRandomization;
                        break;
                    case FormationType.Rectangular:
                        pos = CrowdUtils.CalculateFinishPositionWithIndex(i,Count,circularInterval*1.5f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                memberList[i].destinationPos = pos;
            }
        }
    
        private void PlayParticle(int memberIndex)
        {
            var particle = PoolManager.Instance.Instantiate<CrowdMemberParticle>(particlePrefab);
            particle.transform.position = memberList[memberIndex].transform.position+Vector3.up*1.5f;
            particle.Play(type);
        }
    
        private void SpawnInitial()
        {
            Spawn(initialCount);
        }
    }
}
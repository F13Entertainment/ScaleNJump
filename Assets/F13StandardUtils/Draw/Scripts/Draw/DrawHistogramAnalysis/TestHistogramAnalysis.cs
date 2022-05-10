using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis
{

    [System.Serializable]
    public class TestFailPair
    {
        [PreviewField] public Texture2D test;
        [PreviewField] public Texture2D similar;
    }
    [System.Serializable]
    public class TestResultGroup
    {
        public DrawTypes id;
        public int groupTotalCount;
        public int successCount;
        [ShowInInspector] public float SuccessRatio => (float) successCount / groupTotalCount;
        [ShowInInspector] public int FailCount => fails.Count;
        public List<TestFailPair> fails=new List<TestFailPair>();
    }
    public class TestHistogramAnalysis:MonoBehaviour
    {
        public TextureGroupData testData;
        [ShowInInspector] public int TotalTestCount => resultGroups.Sum(r => r.groupTotalCount);
        [ShowInInspector] public int SuccessTestCount => resultGroups.Sum(r => r.successCount);
        [ShowInInspector] public int FailTestCount => TotalTestCount-SuccessTestCount;
        [ShowInInspector] public float SuccessTestRatio => TotalTestCount>0?(float)SuccessTestCount/TotalTestCount:0f;
        [ReadOnly] public List<TestResultGroup> resultGroups=new List<TestResultGroup>();
        [ReadOnly] public float processTime;


        [Button]
        public void StartTestData()
        {
            var timeA = Time.realtimeSinceStartup;

            resultGroups.Clear();

            foreach (var testDataGroup in testData.groups)
            {
                var resultGroup=new TestResultGroup();
                resultGroup.id = testDataGroup.id;
                resultGroup.groupTotalCount = testDataGroup.textureSet.Count;
                foreach (var texture2D in testDataGroup.textureSet)
                {
                    HistogramAnalysisController.Instance.test = texture2D;
                    var recognizedId = HistogramAnalysisController.Instance.Recognize();
                    if (recognizedId.Equals(resultGroup.id))
                    {
                        resultGroup.successCount++;
                    }
                    else
                    {
                        resultGroup.fails.Add(new TestFailPair()
                        {
                            test = HistogramAnalysisController.Instance.test,
                            similar = HistogramAnalysisController.Instance.similarTrain
                        });
                    }
                }
                resultGroups.Add(resultGroup);
            }
            var timeB = Time.realtimeSinceStartup;
            processTime = timeB - timeA;
        }
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawMatrixRecognizer
{
    
    [System.Serializable]
    public class TestFailPair
    {
        [PreviewField] public Texture2D test;
        public DrawTypes wrongClassify;
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
    
    public class TestMatrixRecognizer : MonoBehaviour
    {
        public TextureGroupData testData;
        [ShowInInspector] public int TotalTestCount => resultGroups.Sum(r => r.groupTotalCount);
        [ShowInInspector] public int SuccessTestCount => resultGroups.Sum(r => r.successCount);
        [ShowInInspector] public int FailTestCount => TotalTestCount-SuccessTestCount;
        [ShowInInspector] public float SuccessTestRatio => TotalTestCount>0?(float)SuccessTestCount/TotalTestCount:0f;
        [ReadOnly] public List<TestResultGroup> resultGroups=new List<TestResultGroup>();
        [ReadOnly] public float processTime;

        
        private Dictionary<Texture2D,List<Texture2D>> resizeDictionary=new Dictionary<Texture2D, List<Texture2D>>();

        [Button]
        public void PrepareResizeDictionary()
        {
            var timeA = Time.realtimeSinceStartup;
            
            resizeDictionary.Clear();
            foreach (var testDataGroup in testData.groups)
            {
                foreach (var texture2D in testDataGroup.textureSet)
                {
                    DrawRecognizerWithMatrix.Instance.test = texture2D;
                    DrawRecognizerWithMatrix.Instance.FillResizeList();
                    var resizeList = DrawRecognizerWithMatrix.Instance.resizeList.ToList();
                    resizeDictionary.Add(texture2D,resizeList);
                }
            }
            var timeB = Time.realtimeSinceStartup;
            processTime = timeB - timeA;
        }
        
        [Button]
        public void StartTestDataWithPreparedResizeDictionary()
        {
            if(!resizeDictionary.Any()) throw new ArgumentNullException("ResizeDictionary is empty. Please fill resize dictionary with "+nameof(PrepareResizeDictionary));
            var timeA = Time.realtimeSinceStartup;

            resultGroups.Clear();

            foreach (var testDataGroup in testData.groups)
            {
                var resultGroup=new TestResultGroup();
                resultGroup.id = testDataGroup.id;
                resultGroup.groupTotalCount = testDataGroup.textureSet.Count;
                foreach (var texture2D in testDataGroup.textureSet)
                {
                    var resizeList = resizeDictionary[texture2D];
                    DrawRecognizerWithMatrix.Instance.resizeList = resizeList;
                    DrawRecognizerWithMatrix.Instance.CalculateSimilariyListAndClassify();
                    var recognizedId = DrawRecognizerWithMatrix.Instance.id;
                    if (recognizedId.Equals(resultGroup.id))
                    {
                        resultGroup.successCount++;
                    }
                    else
                    {
                        resultGroup.fails.Add(new TestFailPair()
                        {
                            test = DrawRecognizerWithMatrix.Instance.test,
                            wrongClassify = DrawRecognizerWithMatrix.Instance.id
                        });
                    }
                }
                resultGroups.Add(resultGroup);
            }
            var timeB = Time.realtimeSinceStartup;
            processTime = timeB - timeA;
        }


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
                    DrawRecognizerWithMatrix.Instance.test = texture2D;
                    var recognizedId = DrawRecognizerWithMatrix.Instance.Recognize();
                    if (recognizedId.Equals(resultGroup.id))
                    {
                        resultGroup.successCount++;
                    }
                    else
                    {
                        resultGroup.fails.Add(new TestFailPair()
                        {
                            test = DrawRecognizerWithMatrix.Instance.test,
                            wrongClassify = DrawRecognizerWithMatrix.Instance.id
                        });
                    }
                }
                resultGroups.Add(resultGroup);
            }
            var timeB = Time.realtimeSinceStartup;
            processTime = timeB - timeA;
        }


        public bool isStopCalibrate = false;
        
        [Button]
        public void CalibrateAll(int iteration, bool calibrateMinSuccessFirst)
        {
            if(!Application.isPlaying) return;
            if(!DrawRecognizerWithMatrix.Instance) return;
            StartCoroutine(CalibrateAllCoroutine(iteration,calibrateMinSuccessFirst));

        }
        
        

        private IEnumerator CalibrateAllCoroutine(int iteration, bool calibrateMinSuccessFirst=true)
        {
            isStopCalibrate = false;
            PrepareResizeDictionary();
            var iterateCount = iteration;
            while (iterateCount>0 && !isStopCalibrate)
            {
                StartTestDataWithPreparedResizeDictionary();
                DrawTypes drawType = (DrawTypes)(iterateCount % Enum.GetValues(typeof(DrawTypes)).Length);
                if (calibrateMinSuccessFirst)
                {
                    var methodBool = iterateCount % 2 == 0;
                    drawType =methodBool? FindMostMistaken():FindWorstSuccessRate();
                }
                var drawMatrixData = DrawRecognizerWithMatrix.Instance.drawMatrixData.Find(d=>d.drawType==drawType);
                yield return StartCoroutine(CalibrateCoroutine(drawMatrixData, 1," remaining iteration: "+iterateCount));
                iterateCount--;
            }
            StartTestDataWithPreparedResizeDictionary();
            Debug.Log("CalibrateAllCoroutine completed final successRate: "+SuccessTestRatio);

        }

        private DrawTypes FindMostMistaken()
        {
            var enumCount = Enum.GetValues(typeof(DrawTypes)).Length;

            var list = new List<int>();
            for (var i = 0; i < enumCount; i++)
            {
                list.Add(0);
            }

            foreach (var testResultGroup in resultGroups)
            {
                foreach (var testFailPair in testResultGroup.fails)
                {
                    var index = (int) testFailPair.wrongClassify;
                    list[index]++;
                }
            }

            var maxWrongClassifyCount = list.Max();
            var indexOf = list.IndexOf(maxWrongClassifyCount);
            var drawType = (DrawTypes) indexOf;
            return drawType;
        }

        private DrawTypes FindWorstSuccessRate()
        {
            var minSuccess = resultGroups.Min(r=>r.SuccessRatio);
            var worstSuccessGroup = resultGroups.Find(r=>r.SuccessRatio==minSuccess);
            return worstSuccessGroup.id;
        }


        [Button]
        public void Calibrate(DrawMatrixData matrixData,int iteration=1)
        {
            if(!Application.isPlaying) return;
            if(!DrawRecognizerWithMatrix.Instance) return;

            StartCoroutine(CalibrateCoroutine(matrixData,iteration));
        }

        private IEnumerator CalibrateCoroutine(DrawMatrixData matrixData,int iteration, string processTag="")
        {
            PrepareResizeDictionary();
            for (int iterate = 0; iterate < iteration; iterate++)
            {
                StartTestDataWithPreparedResizeDictionary();
                bool[] latest = matrixData.matrixSource;
                float maxSuccessRate = SuccessTestRatio;
                Debug.Log(matrixData.name + " data before calibrate success ratio" + maxSuccessRate+" (iteration "+iterate+")");
                bool[] tmp = matrixData.matrixSource.ToArray();
                for (var i = 0; i < tmp.Length; i++)
                {
                    if (isStopCalibrate) break;
                    if(i<matrixData.cols || i+matrixData.cols>=matrixData.rows*matrixData.cols || i%matrixData.cols==0 || i%matrixData.cols == matrixData.cols-1) continue;
                    
                    tmp[i] = !tmp[i];
                    matrixData.matrixSource = tmp;
                    matrixData.SyncSourceToMatrix();
                    StartTestDataWithPreparedResizeDictionary();
                    var successRatio = SuccessTestRatio;
                    yield return new WaitForEndOfFrame();
                
                    if (maxSuccessRate < successRatio)
                    {
                        maxSuccessRate = successRatio;
                        latest = tmp.ToArray();
                        yield return new WaitForEndOfFrame();
                    }
                    else if (maxSuccessRate == successRatio)
                    {
                        var rand = UnityEngine.Random.Range(0,1)==0;
                        if (rand)
                        {
                            tmp[i] = !tmp[i];
                            matrixData.matrixSource = tmp;
                            matrixData.SyncSourceToMatrix();
                            yield return new WaitForEndOfFrame();
                        }
                        else
                        {
                            latest = tmp.ToArray();
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    else
                    {
                        tmp[i] = !tmp[i];
                        matrixData.matrixSource = tmp;
                        matrixData.SyncSourceToMatrix();
                        yield return new WaitForEndOfFrame();
                    }

                    Debug.Log(matrixData.name + "Calibration process: "+ (float)i/tmp.Length+" success rate: "+maxSuccessRate +" (iteration "+iterate+") "+processTag);
                    yield return new WaitForEndOfFrame();
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.SaveAssets();
                    UnityEditor.AssetDatabase.Refresh();
                    yield return new WaitForEndOfFrame();
#endif

                }

                matrixData.matrixSource = latest;
                matrixData.SyncSourceToMatrix();
                Debug.Log(name + " data calibrated: max success rate:" + maxSuccessRate+" (iteration "+iterate+")");
            }
            
        }
        
        
    }
}

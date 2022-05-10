using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Draw.Scripts.Draw.Drawing;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis
{
    [System.Serializable]
    public class HistogramGroup
    {
        public DrawTypes id;
        public List<IntList> histogramSet=new List<IntList>();
    }

    [System.Serializable]
    public class IntList
    {
        public List<int> list=new List<int>();
    }
    
    [System.Serializable] public class DrawTypeEvent:UnityEvent<DrawTypes>{}

    public class HistogramAnalysisController : Singleton<HistogramAnalysisController>
    {
        public TextureGroupData trainData;
        [SerializeField,ReadOnly] private List<HistogramGroup> trainHistogramGroups=new List<HistogramGroup>();
        public int resizeWidth = 32;
        public int resizeHeight = 32;
        public DrawTypeEvent OnClassificationCompleted=new DrawTypeEvent();
        [PreviewField]public Texture2D test;
        [ReadOnly,PreviewField] public Texture2D resizedTest;
        [ReadOnly,PreviewField] public Texture2D similarTrain;
        [ReadOnly] public DrawTypes id;
        [ReadOnly] public List<float> similarityGroupList=new List<float>();
        [ReadOnly] public float processTime;
        

        private void Awake()
        {
            PreCalculateTrainGroupHistograms();
        }

        private void OnEnable()
        {
            DrawController.Instance.OnDrawed.AddListener(OnDrawed);
        }
        
        private void OnDisable()
        {
            DrawController.Instance?.OnDrawed.RemoveListener(OnDrawed);

        }
        
        private void OnDrawed(Texture2D drawed)
        {
            test = drawed.CropActiveArea().ExpandTexture(2f,Color.clear);
            Recognize();
        }


        [Button]
        public DrawTypes Recognize()
        {
            var timeA = Time.realtimeSinceStartup;
            RecognizeProcess();
            var timeB = Time.realtimeSinceStartup;
            processTime = timeB - timeA;
            return id;
        }
        
        private void PreCalculateTrainGroupHistograms()
        {
            foreach (var trainGroup in trainData.groups)
            {
                var precalculatedGroup = new HistogramGroup {id = trainGroup.id};
                foreach (var texture2D in trainGroup.textureSet)
                {
                    var intList=new IntList();
                    intList.list=GetTextureHistogram(texture2D);
                    precalculatedGroup.histogramSet.Add(intList);
                }

                trainHistogramGroups.Add(precalculatedGroup);
            }
        }
        
        private void RecognizeProcess()
        {
            resizedTest = test.ResizeBlit(resizeWidth, resizeHeight);
            similarityGroupList.Clear();

            var testHistogram = GetTextureHistogram(resizedTest);
            var minDistance = float.MaxValue;
            for (var groupIndex = 0; groupIndex < trainHistogramGroups.Count; groupIndex++)
            {
                var group = trainHistogramGroups[groupIndex];
                var similarityList = new List<float>();
                for (var index = 0; index < group.histogramSet.Count; index++)
                {
                    var histogram = group.histogramSet[index];
                    var distance = HistogramSimilarityDistance(testHistogram, histogram.list);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        similarTrain = trainData.groups[groupIndex].textureSet[index];
                    }

                    similarityList.Add(distance);
                }

                var inGroupMin = similarityList.Min();
                var indexGroupMin = similarityGroupList.IndexOf(inGroupMin);
                similarityGroupList.Add(inGroupMin);
            }

            var min = similarityGroupList.Min();
            var indexOf = similarityGroupList.IndexOf(min);
            id = trainData.groups[indexOf].id;
            OnClassificationCompleted.Invoke(id);
        }
        
        public float TextureSimilarityDistance(Texture2D t1, Texture2D t2)
        {
            var similarity = TextureSimilarityArray(t1,t2);
            var sum = similarity.Sum();
            return sum;
        }
        
        public float HistogramSimilarityDistance(List<int> h1, List<int> h2)
        {
            var similarity = HistorgramSimilarityArray(h1,h2);
            var sum = similarity.Sum();
            return sum;
        }
    
        
        public List<float> TextureSimilarityArray(Texture2D t1,Texture2D t2)
        {
            var h1 = GetTextureHistogram(t1);
            var h2 = GetTextureHistogram(t2);
            var similarityArray = HistorgramSimilarityArray(h1, h2);
            return similarityArray;
        }
        
        public List<float> HistorgramSimilarityArray(List<int> h1,List<int> h2)
        {
            var similarityArray= new List<float>(new float[256]);

            for (int i = 0; i < 256; i++)
            {
                similarityArray[i] = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(h1[i], 2) - Mathf.Pow(h2[i], 2)));
            }
            return similarityArray;
        }
    

        public List<int> GetTextureHistogram(Texture2D t)
        {
            var histogram= new List<int>(new int[256]);
            for (var x = 0; x < t.width; x++)
            {
                for (var y = 0; y < t.height; y++)
                {
                    var histVal = GetPixelHistogramValue(t,x, y);
                    histogram[histVal] += 1;
                }
            }
            return histogram;
        }
    
    
        // x   -1 0 1
        //
        //      0 1 2   -1
        //      7 8 3    0
        //      6 5 4    1
        //               y
        public int GetPixelHistogramValue(Texture2D t, int x, int y)
        {
            var pixel0 = GetPixelGrayscaleValue(t,x - 1, y - 1);
            var pixel1 = GetPixelGrayscaleValue(t,x + 0, y - 1);
            var pixel2 = GetPixelGrayscaleValue(t,x + 1, y - 1);
            var pixel3 = GetPixelGrayscaleValue(t,x + 1, y + 0);
            var pixel4 = GetPixelGrayscaleValue(t,x + 1, y + 1);
            var pixel5 = GetPixelGrayscaleValue(t,x + 0, y + 1);
            var pixel6 = GetPixelGrayscaleValue(t,x - 1, y + 1);
            var pixel7 = GetPixelGrayscaleValue(t,x - 1, y + 0);
            // var pixel8 = GetPixelGrayscaleValue(t,x + 0, y + 0);
            var pixel8 = 0.5f;

        
            int b0 = pixel0> pixel8?1:0;
            int b1 = pixel1> pixel8?1:0;
            int b2 = pixel2> pixel8?1:0;
            int b3 = pixel3> pixel8?1:0;
            int b4 = pixel4> pixel8?1:0;
            int b5 = pixel5> pixel8?1:0;
            int b6 = pixel6> pixel8?1:0;
            int b7 = pixel7> pixel8?1:0;

            int result = b0 * 1 +
                         b1 * 2 +
                         b2 * 4 +
                         b3 * 8 +
                         b4 * 16 +
                         b5 * 32 +
                         b6 * 64 +
                         b7 * 128;
            return result;
        }

        public float GetPixelGrayscaleValue(Texture2D t, int x, int y)
        {
            if (x < 0 || y < 0) return Color.black.grayscale;
            return t.GetPixel(x, y).grayscale;
        }
        
    }
}
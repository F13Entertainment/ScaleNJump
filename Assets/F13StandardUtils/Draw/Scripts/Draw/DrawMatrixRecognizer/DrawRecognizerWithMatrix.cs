using System;
using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using F13StandardUtils.Draw.Scripts.Draw.Drawing;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawMatrixRecognizer
{
    public class DrawRecognizerWithMatrix : Singleton<DrawRecognizerWithMatrix>
    {
        [SerializeField] public List<DrawMatrixData> drawMatrixData=new List<DrawMatrixData>();
        [PreviewField] public Texture2D test;
        [ReadOnly,PreviewField] public List<Texture2D> resizeList=new List<Texture2D>();
        [ReadOnly] public List<float> similarityGroupList=new List<float>();
        [ReadOnly] public DrawTypes id;
        [ReadOnly] public float processTime;
        public DrawTypeEvent OnClassificationCompleted=new DrawTypeEvent();
        
        [TitleGroup("Settings"),SerializeField]
        private float similarPixelThresh = 0.5f;


        public bool IsClassificationValid => similarityGroupList.Count == drawMatrixData.Count;
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
            test = drawed;
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

        private void RecognizeProcess()
        {
            FillResizeList();
            CalculateSimilariyListAndClassify();
        }
        
        public void FillResizeList()
        {
            resizeList.Clear();
            var cropped = test.CropActiveArea();
            for (var index = 0; index < drawMatrixData.Count; index++)
            {
                var matrixData = drawMatrixData[index];
                var rows = matrixData.rows;
                var cols = matrixData.cols;
                var expandX = (1f / (cols - 2)) * cols;
                var expandY = (1f / (rows - 2)) * rows;
                var resized = cropped.ExpandTexture(expandX, expandY, Color.clear).ResizeBlit(cols, rows);
                resizeList.Add(resized);
            }
        }

        public void CalculateSimilariyListAndClassify()
        {
            CalculateSimilarityList();
            Classify();
        }


        
        private void CalculateSimilarityList()
        {
            similarityGroupList.Clear();
            for (var index = 0; index < drawMatrixData.Count; index++)
            {
                var matrixData = drawMatrixData[index];
                var resized = resizeList[index];
                var similarity = CalculateSimilarity(resized, matrixData);
                similarityGroupList.Add(similarity);
            }
        }

        private void Classify()
        {
            var max = similarityGroupList.Max();
            var maxIndex = similarityGroupList.IndexOf(max);
            id = drawMatrixData[maxIndex].drawType;
            OnClassificationCompleted.Invoke(id);
        }
        
        public float CalculateSimilarity(Texture2D tex, DrawMatrixData matrixData)
        {
            var rows = matrixData.rows;
            var cols = matrixData.cols;
            if(tex.width!=cols || tex.height!=rows) 
                throw new ArgumentOutOfRangeException("DrawRecognizer.CalculateSimilarity() Error: tex.width!= matrix.cols || tex.height!= matrix.rows");

            var similarity = 0f;
            var similarityPerCell = 1f / (rows * cols);
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var isFilledOnMatrix = matrixData.GetValue(x,y);
                    var isFilledOnTexture = tex.GetPixel(x, rows-1-y).grayscale >= similarPixelThresh;
                    if (isFilledOnMatrix == isFilledOnTexture) 
                        similarity += similarityPerCell;

                }
            }
            return similarity;
        }

        [Button]
        public float GetSimilarity(DrawTypes drawType)
        {
            return similarityGroupList[(int)drawType];
        }


        [Button]
        private void Test(DrawTypes drawType)
        {
            test = TestImage(drawType);
            Recognize();
        }
        
        private Texture2D TestImage(DrawTypes drawType)
        {
            var matrixData = drawMatrixData.Find(d=>d.drawType==drawType);
            var rows = matrixData.rows;
            var cols = matrixData.cols;
            var tex= new Texture2D(cols,rows);
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var isFilledOnMatrix = matrixData.GetValue(x,y);
                    tex.SetPixel(x,rows-1-y,isFilledOnMatrix?Color.white : Color.black);
                }
            }
            tex.Apply();
            return tex;
        }
        
        
    }
}
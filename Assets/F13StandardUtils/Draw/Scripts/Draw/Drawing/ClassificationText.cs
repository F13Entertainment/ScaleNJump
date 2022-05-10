using System;
using System.Collections;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using F13StandardUtils.Draw.Scripts.Draw.DrawMatrixRecognizer;
using TMPro;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{

    public enum DrawClassifyMethod
    {
        HistogramAnalysis,
        DrawMatrix
    }
    
    public class ClassificationText : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [SerializeField] private DrawClassifyMethod method;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        

        private void OnEnable()
        {
            DrawController.Instance.OnDrawFailed.AddListener(OnDrawFailed);
            HistogramAnalysisController.Instance?.OnClassificationCompleted.AddListener(OnHistogramClassificationCompleted);
            DrawRecognizerWithMatrix.Instance.OnClassificationCompleted.AddListener(OnMatrixClassificationCompleted);
        }
        private void OnDisable()
        {
            DrawController.Instance?.OnDrawFailed.AddListener(OnDrawFailed);
            HistogramAnalysisController.Instance?.OnClassificationCompleted.RemoveListener(OnHistogramClassificationCompleted);
            DrawRecognizerWithMatrix.Instance?.OnClassificationCompleted.RemoveListener(OnMatrixClassificationCompleted);
            
        }
        
        private void OnDrawFailed()
        {
            _text.text = "Try Again!";
        }
        
        private void OnHistogramClassificationCompleted(DrawTypes className)
        {
            if(method==DrawClassifyMethod.HistogramAnalysis)
                StartCoroutine(UpdateText());
        }

        private void OnMatrixClassificationCompleted(DrawTypes className)
        {
            if(method==DrawClassifyMethod.DrawMatrix)
                StartCoroutine(UpdateText());
        }

        private IEnumerator UpdateText()
        {
            _text.text = string.Empty;
            yield return null;
            switch (method)
            {
                case DrawClassifyMethod.HistogramAnalysis:
                    _text.text = method.ToString()+" : "+HistogramAnalysisController.Instance.id.ToString()+
                                 "\n"+HistogramAnalysisController.Instance.processTime.ToString("0.000")+"ms";
                    
                    break;
                case DrawClassifyMethod.DrawMatrix:
                    _text.text = method.ToString()+" : "+DrawRecognizerWithMatrix.Instance.id.ToString()+
                                 "\n"+DrawRecognizerWithMatrix.Instance.processTime.ToString("0.000")+"ms";
                    _text.text += "\n"+"<mspace=0.75em>" + DrawMatrixToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }

        private string DrawMatrixToString()
        {
            var str = string.Empty;
            str = "\n";
            for (var index = 0; index < DrawRecognizerWithMatrix.Instance.drawMatrixData.Count; index++)
            {
                var drawMatrixData = DrawRecognizerWithMatrix.Instance.drawMatrixData[index];
                var rows = drawMatrixData.rows;
                var cols = drawMatrixData.cols;
                str += drawMatrixData.drawType.ToString() + "\n";
                for (int x = 0; x < cols; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        var isFilledOnMatrix = drawMatrixData.GetValue(y,x);
                        str+=isFilledOnMatrix ? "<color=red>1</color=red>" : "0";
                    }
                    str += "\n";
                }
            }

            return str;
        }
    }
}

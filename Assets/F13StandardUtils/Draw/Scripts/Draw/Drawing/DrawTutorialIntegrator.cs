using System.Linq;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using F13StandardUtils.Scripts.Core;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public class DrawTutorialIntegrator : MonoBehaviour
    {
        [SerializeField] private DrawTutorial drawTutorial;
    
        private void OnEnable()
        {
            OnLevelInit(0);
        }
    
        private void Awake()
        {
            GameController.Instance.OnLevelInit.AddListener(OnLevelInit);
        }
    
        private void OnDestroy()
        {
            GameController.Instance?.OnLevelInit.RemoveListener(OnLevelInit);
        }
    
        private void OnLevelInit(int l)
        {
            drawTutorial.DisableAll();
            Invoke(nameof(OnLevelInitProcess), Time.deltaTime);
        }

        private void OnLevelInitProcess()
        {
            var drawType = FindNearestDrawType();
            SetTutorial(drawType);
        }

        private void SetTutorial(DrawTypes drawType)
        {
            drawTutorial.SetType(drawType);
        }

        private DrawTypes FindNearestDrawType()
        {
            // var gateList = FindObjectsOfType<BaseGate>();
            // if (!gateList.Any())
                return DrawTypes.Curl;
            // int minIndex = 0;
            //
            // for (var i = 1; i < gateList.Length; i++)
            // {
            //     var baseGate = gateList[i];
            //     var minGate = gateList[minIndex];
            //     var z = baseGate.transform.position.z;
            //     if (z < minGate.transform.position.z)
            //     {
            //         minIndex = i;
            //     }
            // }
            //
            // return gateList[minIndex].DrawType;
        }
    }
}

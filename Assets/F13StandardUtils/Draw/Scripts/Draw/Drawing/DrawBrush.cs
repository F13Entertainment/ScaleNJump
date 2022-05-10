using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using F13StandardUtils.Draw.Scripts.Draw.DrawMatrixRecognizer;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public class DrawBrush : MonoBehaviour
    {
        [SerializeField] private List<LineRenderer> _lines;
        [SerializeField] private GameObject _particle;
        public Color Color => _lines.First().startColor;

        [SerializeField,OnValueChanged(nameof(UpdateUseWorldSpace))] private bool _useWorldSpace;
        public bool UseWorldSpace => _useWorldSpace;

        private void UpdateUseWorldSpace()
        {
            _lines.ForEach(l => l.useWorldSpace = _useWorldSpace);
        }


        [SerializeField] private DrawPopup _drawPopup;


        private void OnEnable()
        {
            GameController.Instance?.OnLevelInit.AddListener(OnLevelInit);
            DrawController.Instance.OnDrawStart.AddListener(OnDrawStart);
            DrawController.Instance.OnDrawFailed.AddListener(OnDrawFailed);
            DrawController.Instance.OnDrawSuccess.AddListener(OnDrawSuccess);
        }
    
        private void OnDisable()
        {
            GameController.Instance?.OnLevelInit.RemoveListener(OnLevelInit);
            DrawController.Instance?.OnDrawStart.RemoveListener(OnDrawStart);
            DrawController.Instance?.OnDrawFailed.RemoveListener(OnDrawFailed);
            DrawController.Instance?.OnDrawSuccess.RemoveListener(OnDrawSuccess);
        }
    
        private void OnDrawStart()
        {
            StartDrawAnimation();
        }
    
        private void OnDrawFailed()
        {
            FailDrawAnimation();
        }
    
        private void OnDrawSuccess()
        {
            var drawType = DrawRecognizerWithMatrix.Instance.id;
            if (DrawRecognizerWithMatrix.Instance.GetSimilarity(drawType)>0.75f)
            {
                SuccessDrawAnimation(drawType);
            }
            else if (DrawRecognizerWithMatrix.Instance.GetSimilarity(drawType)>0.65f)
            {
                NotrDrawAnimation();
            }
            else
            {
                FailDrawAnimation();            
            }
        }

        public int PositionCount
        {
            get
            {
                return _lines.First().positionCount;
            }

            set
            {
                foreach (var line in _lines)
                {
                    line.positionCount = value;
                }
            }
        }

        public void SetUseWorldSpace(bool val)
        {
            _useWorldSpace = val;
            UpdateUseWorldSpace();
        }

        public void SetPosition(int positionIndex, Vector3 pointPos)
        {
            foreach (var line in _lines)
            {
                line.SetPosition(positionIndex,pointPos);
            }
        }
    
        public void SetColor(Color color)
        {
            var lineRenderer = _lines.First();
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    


        public void StartDrawAnimation()
        {
            SetColor(Color.white);

        }

        public void FailDrawAnimation()
        {
            ResetPerfectCounter();
            SetColor(Color.red);
            // iOSHapticController.instance.TriggerNotificationWarning();
        }

        private int perfectCounter = 0;
        public void NotrDrawAnimation()
        {
            SetColor(Color.white);
            ShowParticle();
            ResetPerfectCounter();
            _drawPopup.SpawnPopupText("GOOD!", Color);
            // iOSHapticController.instance.TriggerImpactLight();
        }

        public void SuccessDrawAnimation(DrawTypes drawType)
        {
            SetColor(DrawController.Instance.brushColors[(int)drawType]);
            ShowParticle();
            perfectCounter++;
            _drawPopup.SpawnPopupText(PerfectMessage(),Color);
            // iOSHapticController.instance.TriggerImpactMedium();
        }

        private void ShowParticle()
        {
            var lineRenderer = _lines.First();
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pos = lineRenderer.GetPosition(i);
                var particle = PoolManager.Instance.Instantiate<DrawParticle>(_particle);
                particle.transform.position = pos;
                particle.Play(Color);
            }
        }

        private string PerfectMessage()
        {
            if (perfectCounter < 2) return "PERFECT! ";
            else if (perfectCounter < 3) return "PERFECT! " + perfectCounter + "x";
            else if (perfectCounter < 4) return "WOW!";
            else if (perfectCounter < 5) return "OMG!";
            else if (perfectCounter < 6) return "YOU ARE ARTIST!";
            else if (perfectCounter < 7) return "PICASSO!";
            else if (perfectCounter < 8) return "MICHALENGELO!";
            else if (perfectCounter < 9) return "LEONARDO DA VINCI! ";
            else if (perfectCounter < 10) return "+RESPECT";
            else if (perfectCounter < 11) return "ENTER THE LIMIT!";
            else if (perfectCounter < 12) return "HIGHWAY TO HELL!";
            else if (perfectCounter < 13) return "SATAN!";
            else return "F13 GOD!";
        }
    
        private void OnLevelInit(int l)
        {
            ResetPerfectCounter();
        }

        private void ResetPerfectCounter()
        {
            perfectCounter = 0;
        }
    }
}

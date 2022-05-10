using System;
using System.Collections;
using System.Collections.Generic;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    [System.Serializable]
    public class DrawEvent:UnityEvent<Texture2D>{}

    public class DrawController : Singleton<DrawController>
    {
        [ShowInInspector] public static int triggerPointThresh=5;
        [ShowInInspector] public static float updateInterval = 0.2f;
        [SerializeField,ReadOnly] private bool _isDrawing = false;
        [SerializeField,ReadOnly] private float lastUpdateTime;
        [SerializeField,ReadOnly] private float lastDrawDuration;
        public bool isDraw = true;
        
        public List<Color> brushColors=new List<Color>();
        public GameObject brush;
        public RenderTexture drawTexture;
        private DrawBrush currentDrawBrush;
        private Vector3 lastPos;
        [SerializeField,PreviewField] private Texture2D _drawed;
        public Texture2D Drawed => _drawed;

        public UnityEvent OnDrawStart =new UnityEvent();
        public UnityEvent OnDrawEnd = new UnityEvent();
        public UnityEvent OnDrawSuccess =new UnityEvent();
        public UnityEvent OnDrawFailed = new UnityEvent();
        public DrawEvent OnDrawed=new DrawEvent();
        
        public bool savePNG = false;
        public bool isUpdateMode = false;

        public bool IsDrawing => _isDrawing;

        public float LastDrawDuration => lastDrawDuration;
        public float LastUpdateTime => lastUpdateTime;


        private void Update()
        {
            Drawing();
        }


        private void Drawing() 
        {
            if(!isDraw) return;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ClearBrush();
                _isDrawing = true;
                lastDrawDuration = 0;
                OnDrawStart.Invoke();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(_isDrawing) SaveTexture();
                StartCoroutine(ClearBrushCoroutine());
            }
            else if (Input.GetKey(KeyCode.Mouse0) && _isDrawing)
            {
                lastDrawDuration += Time.deltaTime;
                PointToMousePos();
                if (isUpdateMode && (Time.time-lastUpdateTime) >= updateInterval)
                {
                    lastUpdateTime = Time.time;
                    SaveTexture();
                }
            }
        }

        public void ForceStopDrawing()
        {
            OnDrawFailed.Invoke();
            StartCoroutine(ClearBrushCoroutine());
        }

        private void ClearBrush() 
        {
            if (currentDrawBrush)
            {
                currentDrawBrush.PositionCount = 0;
            }
            else
            {
                GameObject brushInstance = Instantiate(brush);
                currentDrawBrush = brushInstance.GetComponent<DrawBrush>();
                if(!currentDrawBrush.UseWorldSpace)
                    currentDrawBrush.transform.SetParent(Camera.main.transform,false);
            }
        }
    
        private void PointToMousePos() 
        {
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var point = hit.point;
                if(!currentDrawBrush.UseWorldSpace)
                    point=Camera.main.transform.InverseTransformPoint(point);
                if (point != lastPos)
                {
                    AddAPoint(point);
                    lastPos = point;
                }
            }

        }

        private void AddAPoint(Vector3 pointPos) 
        {
            currentDrawBrush.PositionCount++;
            int positionIndex = currentDrawBrush.PositionCount - 1;
            currentDrawBrush.SetPosition(positionIndex, pointPos);
        }
    
        private void SaveTexture()
        {
            StartCoroutine(SaveTextureCoroutine());
        }

        private IEnumerator SaveTextureCoroutine()
        {
            if (currentDrawBrush.PositionCount > triggerPointThresh)
            {
                yield return new WaitForEndOfFrame();
                RenderTexture.active = drawTexture;
                var texture2D = new Texture2D(drawTexture.width, drawTexture.height);
                texture2D.ReadPixels(new Rect(0, 0, drawTexture.width, drawTexture.height), 0, 0);
                texture2D.Apply();
                _drawed = texture2D;
                if(savePNG) _drawed.CropActiveArea().ExpandTexture(2f,Color.clear).ResizeBlit(32,32).WritePNG("drawed"+DateTime.Now.Ticks);
                OnDrawed.Invoke(_drawed);

                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    OnDrawSuccess.Invoke();
                }
                
            }
            else
            {
                if (!Input.GetKey(KeyCode.Mouse0))
                {
                    OnDrawFailed.Invoke();
                }
            }

        }

        private IEnumerator ClearBrushCoroutine()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _isDrawing = false;
            OnDrawEnd.Invoke();
            yield return new WaitForSeconds(0.4f);
            ClearBrush();
        }
    }
}
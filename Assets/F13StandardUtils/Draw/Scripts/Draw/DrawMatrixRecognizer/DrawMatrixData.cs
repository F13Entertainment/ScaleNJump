using F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawMatrixRecognizer
{
    [CreateAssetMenu(fileName = "DrawMatrixData", menuName = "_GAME/Texture/DrawMatrixData", order = 0)]
    public class DrawMatrixData : SerializedScriptableObject
    {
        public DrawTypes drawType;

        [SerializeField,TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawColoredEnumElement))]
        private bool[,] matrix = new bool[7, 7];


        public bool[] matrixSource;
        [ReadOnly] public int rows;
        [ReadOnly] public int cols;

        public bool GetValue(int x, int y) => matrixSource[y * cols + x];
        
        protected override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            SyncMatrixToSource();
        }

        [Button]
        public void SyncMatrixToSource()
        {
            rows = matrix.GetLength(0);
            cols = matrix.GetLength(1);
            matrixSource = new bool[rows * cols];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var isFilledOnMatrix = matrix[y, x];
                    matrixSource[x * cols + y] = isFilledOnMatrix;
                }
            }
        }

        [Button]
        public void SyncSourceToMatrix()
        {
            rows = (int) Mathf.Sqrt(matrixSource.Length);
            cols = rows;
            matrix=new bool[rows,cols];
            for (var i = 0; i < matrixSource.Length; i++)
            {
                int x = i/cols;
                int y = i % cols;
                matrix[y, x] = matrixSource[i];
            }
        }
        

        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
#if UNITY_EDITOR
            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
#endif
            return value;
        }

    }
}




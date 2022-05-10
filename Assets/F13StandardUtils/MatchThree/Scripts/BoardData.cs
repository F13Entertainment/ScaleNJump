using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace F13StandardUtils.MatchThree.Scripts
{
    public enum TileType
    {
        None=0,
        Standard=1,
        Creator=2
    }
    
    [CreateAssetMenu(fileName = "BoardData", menuName = "_MatchThree/BoardData", order = 0)]
    public class BoardData : SerializedScriptableObject
    {
        public string id;
        [SerializeField,TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawColoredEnumElement))]
        private TileType[,] matrix = new TileType[7, 7];


        [ReadOnly] public TileType[] matrixSource;
        [ReadOnly] public int rows;
        [ReadOnly] public int cols;

        public TileType GetValue(int rowIndex, int colIndex) => matrixSource[rowIndex * cols + colIndex];
        
        [Button]
        protected override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            rows = matrix.GetLength(1);
            cols = matrix.GetLength(0);
            matrixSource = new TileType[rows * cols];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    var isFilledOnMatrix = matrix[x,y];
                    matrixSource[y * cols + x]=isFilledOnMatrix;
                }
            }

        }

        private static Color[] DrawColors =
        {
            new Color(0, 0, 0, 0.5f),
            Color.green,
            Color.cyan, 
        };

        private static TileType DrawColoredEnumElement(Rect rect, TileType value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = (TileType)( ((int)value+1) % Enum.GetValues(typeof(TileType)).Length);
                GUI.changed = true;
                Event.current.Use();
            }
#if UNITY_EDITOR
            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), DrawColors[(int)value]);
#endif
            return value;
        }

    }
}




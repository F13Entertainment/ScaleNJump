using UnityEngine;
using Sirenix.OdinInspector;

namespace Assets.F13SDK.Scripts
{
    [System.Serializable]
    public class Level
    {
        [BoxGroup("Level ınfo")]
        [EnumToggleButtons]
        public LevelType levelType;
        [InlineEditor(InlineEditorModes.LargePreview)]
        public GameObject levelPrefab;
        public int levelId;
        public bool IsLevelCompleted = false;
        public bool IsLevelFailed = false;

    }
}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.DrawHistogramAnalysis
{
    public enum DrawTypes
    {
        Curl,
        I,
        L,
        Lower,
        M,
        N,
        O,
        S,
        U,
        V,
        Z
    }
    
    [System.Serializable]
    public class TextureGroup
    {
        public DrawTypes id;
        public List<Texture2D> textureSet=new List<Texture2D>();
    }
    
    [CreateAssetMenu(fileName = "TextureGroupData", menuName = "_GAME/Texture/TextureGroupData", order = 0)]
    public class TextureGroupData : ScriptableObject
    {
        public List<TextureGroup> groups=new List<TextureGroup>();
     
        [SerializeField] private string path = "Assets/_GAME/Datas/Dataset/Train/";
        [Button]
        private void AutoLoad()
        {
#if UNITY_EDITOR
            foreach (var textureGroup in groups)
            {
                textureGroup.textureSet.Clear();
                string[] paths = System.IO.Directory.GetFiles(path+textureGroup.id.ToString(), "*.png");
                foreach(string p in paths)
                {
                    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(p);
                    textureGroup.textureSet.Add(texture);
                }
            }
#endif
        }
    }
    

}
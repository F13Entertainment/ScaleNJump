using System.IO;
using UnityEngine;

namespace F13StandardUtils.Draw.Scripts.Draw.Drawing
{
    public static class DrawUtils 
    {
        public static Texture2D ResizeBlit(this Texture2D source, int newWidth, int newHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
            nTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            return nTex;
        }

        public static Texture2D CropActiveArea(this Texture2D tex)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = 0;
            int maxY = 0;
            
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    var value = tex.GetPixel(x,y).grayscale;
                    if (value > 0)
                    {
                        if (minX > x) minX = x;
                        if (minY > y) minY = y;
                        if (maxX < x) maxX = x;
                        if (maxY < y) maxY = y;
                    }
                }
            }

            var crop = tex.CropTexture(minX,minY,maxX-minX,maxY-minY);
            return crop;
        }
        
        public static Texture2D CropTexture(this Texture2D tex, int x, int y, int width, int height)
        {
            var cropped=new Texture2D(width,height);
            var newPixels = tex.GetPixels(x, y, width, height);
            cropped.SetPixels(newPixels);
            cropped.Apply();
            return cropped;
        }

        public static Texture2D ExpandTexture(this Texture2D tex, float multiplier, Color fillColor)
        {
            return tex.ExpandTexture(multiplier, multiplier, fillColor);

        }
        
        public static Texture2D ExpandTexture(this Texture2D tex, float multiplierX,float multiplierY,Color fillColor)
        {
            var newWidth = tex.width * multiplierX;
            var newHeight = tex.height * multiplierY;
            var expand=new Texture2D((int)newWidth, (int)newHeight);
            var fillPixels = new Color[expand.width * expand.height];
 
            for (int i = 0; i < fillPixels.Length; i++)
            {
                fillPixels[i] = fillColor;
            }
            expand.SetPixels(0,0,expand.width,expand.height,fillPixels);
            var startX = (newWidth - tex.width) / 2;
            var startY = (newHeight - tex.height) / 2;
            var pixels = tex.GetPixels(0, 0, tex.width, tex.height);
            expand.SetPixels((int)startX, (int)startY, tex.width, tex.height,pixels);
            expand.Apply();
            return expand;
        }
        public static Texture2D BlackWhite(this Texture2D tex, float thresh)
        {
            var bw= new Texture2D(tex.width,tex.height);
            var newPixels = tex.GetPixels(0, 0, tex.width, tex.height);
            for (var i = 0; i < newPixels.Length; i++)
            {
                newPixels[i]=newPixels[i].grayscale>thresh?Color.white:Color.black;
            }
            bw.SetPixels(newPixels);
            bw.Apply();
            return bw;
        }
        
        public static void WritePNG(this Texture2D tex,string name, string folderPath="/F13StandardUtils/Draw/Datas/Draw/Dataset/Test/")
        {
#if UNITY_EDITOR
            var data = tex.EncodeToPNG();
            var path =  folderPath + name + ".png";
            File.WriteAllBytes(Application.dataPath +path, data);
            UnityEditor.AssetDatabase.Refresh();
            var assetPath = "Assets" + path;
            var importer = (UnityEditor.TextureImporter) UnityEditor.AssetImporter.GetAtPath(assetPath);
            importer.isReadable = true;
            UnityEditor.AssetDatabase.ImportAsset( assetPath, UnityEditor.ImportAssetOptions.ForceUpdate );
#endif
        }
    }
}

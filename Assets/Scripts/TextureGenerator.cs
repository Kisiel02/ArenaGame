using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator 
{
   public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point; //usuwa blura
        texture.wrapMode = TextureWrapMode.Clamp; //rogi mapy nie nachodza na siebie
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;

    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);    //mozliwy blad przy prostokatnej teksturze 
            }
        }
        return TextureFromColourMap(colourMap, width, height);
    }
}

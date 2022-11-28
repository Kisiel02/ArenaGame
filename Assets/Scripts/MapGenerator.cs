using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public float meshHeightMultiplier;
    public int seed;
    public Vector2 offset;

    public bool autoUpdate;
    public bool useFalloff;
    float[,] falloffMap;

    public TerrainType[] regions;
    public enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
    public DrawMode drawMode;
    public AnimationCurve meshHeightCurve;

    private void Awake() {
        falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapHeight);
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                if (useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y]) - falloffMap[x, y];
                }

                float currentHeight = noiseMap[x, y];
                for (int i=0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)  //tu mozna zrobic zakres od do 
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));

        }else if(drawMode == DrawMode.ColourMap) {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }else if(drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }else if(drawMode == DrawMode.FalloffMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(Falloff.GenerateFalloffMap(mapWidth, mapHeight)));
        }
    }

    private void OnValidate()
    {
        if(mapWidth < 1) { mapWidth = 1; }
        if(mapHeight < 1) { mapHeight = 1; }
        if(lacunarity < 1) { lacunarity = 1; }
        if(octaves < 0) { octaves = 0; }

        falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapHeight);
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

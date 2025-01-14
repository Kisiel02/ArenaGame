using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public float minHeight
    {
        get { return 10 * meshHeightMultiplier * meshHeightCurve.Evaluate(0); }
    }

    public float maxHeight
    {
        get { return 10 * meshHeightMultiplier * meshHeightCurve.Evaluate(1); }
    }

    public Material terrainMaterial;
    public Color[] baseColours;
    [Range(0, 0.1f)] public float[] baseStartHeights;
    [Range(0, 0.1f)] public float[] baseBlends;


    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)] public float persistance = 0.3f;
    public float lacunarity = 2.0f;

    public float meshHeightMultiplier;
    public int seed;
    public Vector2 offset;

    public bool autoUpdate;
    public bool useFalloff;
    float[,] falloffMap;

    public TerrainType[] regions;

    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh,
        FalloffMap
    };

    public DrawMode drawMode;
    public AnimationCurve meshHeightCurve;
    

    public void GenerateRandomMap()
    {
        seed = Random.Range(100000, 999999);
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (useFalloff)
        {
            falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapHeight);
        }

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance,
            lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y]) - falloffMap[x, y];
                }

                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height) //tu mozna zrobic zakres od do 
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(Falloff.GenerateFalloffMap(mapWidth, mapHeight)));
        }


        UpdateMeshHeights(terrainMaterial, minHeight, maxHeight);
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight < 1)
        {
            mapHeight = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }

        falloffMap = Falloff.GenerateFalloffMap(mapWidth, mapHeight);
    }
    
    //--------------------------------------------------

    private void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {
        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
        material.SetColorArray("baseColours", baseColours);
        material.SetFloatArray("baseStartHeights", baseStartHeights);
        material.SetInt("baseColourCount", baseColours.Length);
        material.SetFloatArray("baseBlends", baseBlends);
    }

    //---------------------------------------------------
}

[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
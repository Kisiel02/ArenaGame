using System;
using BeardedManStudios.Forge.Networking;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour //MapGeneratorBehavior
{
    public static MapGenerator Instance { get; private set; }

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;

    public float meshHeightMultiplier;
    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh
    };

    public DrawMode drawMode;
    public AnimationCurve meshHeightCurve;

    [Header("Script refrences")] [SerializeField]
    //reference to the main networked script
    private NetworkedPlayer np;
    
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GenerateRandomMap()
    {
        this.seed = Random.Range(100, 10000);
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance,
            lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
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
    }
    
}

[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
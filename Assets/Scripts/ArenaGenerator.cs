using System;
using DefaultNamespace;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    public static ArenaGenerator Instance { get; private set; }

    public MapGenerator mapGenerator;

    public SpawnPlayer spawnPlayer;

    public ObjectGenerator objectGenerator;

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

    public void InitArena()
    {
        try
        {
            GenerationParameters generationParameters = GameObject.FindWithTag("GenerationParameters")
                .GetComponent<GenerationParameters>();

            if (generationParameters.useParams)
            {
                copyGenerationParameters(generationParameters);
                GenerateArena();
            }
            else
            {
                GenerateRandomArena();
            }

            Destroy(generationParameters);
        }
        catch (NullReferenceException ex)
        {
            GenerateRandomArena();
        }
    }

    private void copyGenerationParameters(GenerationParameters generationParameters)
    {
        mapGenerator.seed = generationParameters.seed;
        mapGenerator.lacunarity = generationParameters.lacunarity;
        mapGenerator.octaves = generationParameters.octaves;
        mapGenerator.persistance = generationParameters.persistance;
        mapGenerator.useFalloff = generationParameters.falloff;
        mapGenerator.noiseScale = generationParameters.scale;
    }

    public void GenerateRandomArena()
    {
        spawnPlayer = GameObject.Find("SpawnPlayer").GetComponent<SpawnPlayer>();
        mapGenerator.GenerateRandomMap();
        ReplaceSpawns();
        objectGenerator.GenerateObjects(mapGenerator.seed);
    }

    public void GenerateArena()
    {
        spawnPlayer = GameObject.Find("SpawnPlayer").GetComponent<SpawnPlayer>();
        mapGenerator.GenerateMap();
        ReplaceSpawns();
        objectGenerator.GenerateObjects(mapGenerator.seed);
    }

    public void ReplaceSpawns()
    {
        for (int i = 0; i < spawnPlayer.transform.childCount; i++)
        {
            Transform spawnPoint = spawnPlayer.transform.GetChild(i).transform;

            //Put spawn higher to detect floor
            spawnPoint.position =
                new Vector3(spawnPoint.position.x, spawnPoint.position.y + 200f, spawnPoint.position.z);
            Transform newTransform = SurfaceAligner.CalculatePositionAndAddHeight(spawnPoint, 4.5f, true);
            spawnPoint.position = newTransform.position;
            spawnPoint.rotation = newTransform.rotation;
        }
    }
}
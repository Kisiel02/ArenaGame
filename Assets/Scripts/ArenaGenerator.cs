using System;
using DefaultNamespace;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    public static ArenaGenerator Instance { get; private set; }

    public MapGenerator mapGenerator;
    
    public ObjectGenerator objectGenerator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            AttachDependencies();
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
               GenerateArenaFromParams();
               generationParameters.useParams = false;
            }
            else
            {
                GenerateRandomArena();
            }
        }
        catch (NullReferenceException ex)
        {
            GenerateRandomArena();
        }
    }
    
    public void GenerateArenaFromParams()
    {
        GenerationParameters generationParameters = GameObject.FindWithTag("GenerationParameters")
            .GetComponent<GenerationParameters>();
        
        CopyGenerationParameters(generationParameters);
        GenerateArena();
    }

    public void GenerateRandomArena()
    {
        mapGenerator.GenerateRandomMap();
        objectGenerator.GenerateObjects(mapGenerator.seed);
    }

    private void CopyGenerationParameters(GenerationParameters generationParameters)
    {
        mapGenerator.seed = generationParameters.seed;
        mapGenerator.lacunarity = generationParameters.lacunarity;
        mapGenerator.octaves = generationParameters.octaves;
        mapGenerator.persistance = generationParameters.persistance;
        mapGenerator.useFalloff = generationParameters.falloff;
        mapGenerator.noiseScale = generationParameters.scale;
    }


    private void GenerateArena()
    {
        mapGenerator.GenerateMap();
        objectGenerator.GenerateObjects(mapGenerator.seed);
    }
    
    private void AttachDependencies()
    {
        mapGenerator = GetComponent<MapGenerator>();
        objectGenerator = GetComponent<ObjectGenerator>();
    }
}
using System;
using DefaultNamespace;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    public static ArenaGenerator Instance { get; private set; }

    public MapGenerator mapGenerator;

    public SpawnPlayer spawnPlayer;
    
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
    }

    public void GenerateArena()
    {
        spawnPlayer = GameObject.Find("SpawnPlayer").GetComponent<SpawnPlayer>();
        mapGenerator.GenerateMap();
        ReplaceSpawns();
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

    /*public void SpawnMountains()
    {
        Random.InitState(mapGenerator.seed);
        randomNumber = Random.Range(0, numberOfObjects);


        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }


        for (int i = 0; i < numberOfObjects; i++)
        {
            randomScale = Random.Range(20, 30);
            randomPrefab = Random.Range(0, mountainPrefab.Length - 1);
            float x;
            float z;
            Quaternion rot;
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            if (randomPrefab != 5)
                mountainPrefab[randomPrefab].transform.localScale =
                    new Vector3(randomScale + 12, randomScale, randomScale + 12);

            if (i == randomNumber)
            {
                x = Mathf.Cos(angle) * radius * 1.3f;
                z = Mathf.Sin(angle) * radius * 1.3f;
                float angleDegrees = -angle * Mathf.Rad2Deg;
                rot = Quaternion.Euler(0, angleDegrees, 0);
                randomPrefab = 0;
            }
            else if (i == randomNumber + 4 || i == randomNumber + 9)
            {
                x = Mathf.Cos(angle) * radius;
                z = Mathf.Sin(angle) * radius;
                float angleDegrees = -angle * Mathf.Rad2Deg;
                rot = Quaternion.Euler(0, angleDegrees - 120, 0);
                randomPrefab = 5;
            }
            else
            {
                x = Mathf.Cos(angle) * radius;
                z = Mathf.Sin(angle) * radius;
                rot = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            }

            Vector3 pos = transform.position + new Vector3(x, 0, z);

            if (SurfaceAligner.GroundDetector(pos) < 130 && SurfaceAligner.GroundDetector(pos) > 97)
            {
                Instantiate(mountainPrefab[randomPrefab], pos, rot, transform);
            }
        }

        ReplaceObjects();
    }

    public void ReplaceObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform objectTransform = transform.GetChild(i).transform;
            Transform newTransform = SurfaceAligner.CalculatePositionAndAddHeight(objectTransform, -1f, false);
            objectTransform.position = newTransform.position;
        }
    }*/
}
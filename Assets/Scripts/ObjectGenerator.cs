using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mesh;

    [SerializeField] private List<GameObject> objectToSpawn;

    [SerializeField] private int ObjectCount = 15;

    [SerializeField] private int maxX = 60;

    [SerializeField] private int maxZ = 60;

    [SerializeField] private int y = 30;

    [SerializeField] private float minimalY = 3.0f;

    public void GenerateObjects(int seed)
    {
        Random.InitState(seed);
        RemoveObjects();
        PlaceWeapons();
        PlaceSpawnPoints();
        GenerateObjects();
    }

    private void RemoveObjects()
    {
        while (mesh.transform.childCount > 0)
        {
            DestroyImmediate(mesh.transform.GetChild(0).gameObject);
        }
    }

    private void PlaceWeapons()
    {
        ReplaceObjectWithTag("SpawnWeapon", 1.0f);
        MakeChildrenActiveByTag("SpawnWeapon");
    }
    
    private void PlaceSpawnPoints()
    {
        ReplaceObjectWithTag("SpawnPlayer", 2.0f);
    }


    private void ReplaceObjectWithTag(String tag, float raiseValue)
    {
        Transform transform = GameObject.FindWithTag(tag).transform;

        foreach (Transform child in transform)
        {
            PlaceObject(child, raiseValue);
        }
    }

    private void PlaceObject(Transform objectToBePlaced, float raiseValue)
    {
        bool placed = false;

        while (!placed)
        {
            var newPos = RandomiseNewPosition();
            if (newPos.y >= minimalY)
            {
                newPos.y += raiseValue;
                objectToBePlaced.transform.position = newPos;
                placed = true;
            }
        }
    }

    private Vector3 RandomiseNewPosition()
    {
        int x = Random.Range(-maxX, maxX);
        int z = Random.Range(-maxZ, maxZ);
        Vector3 newPos = new Vector3(x, y, z);
        newPos = SurfaceAligner.CalculatePosition(newPos);
        return newPos;
    }

    private void GenerateObjects()
    {
        int objectsSpawned = 0;

        while (objectsSpawned != ObjectCount)
        {
            Vector3 newPos = RandomiseNewPosition();
            GameObject pickedPrefab = objectToSpawn[Random.Range(0, objectToSpawn.Count - 1)];

            if (newPos.y >= minimalY)
            {
                var spawned = Instantiate(pickedPrefab, newPos, Quaternion.identity);
                spawned.transform.parent = mesh.transform;
                objectsSpawned++;
            }
        }
    }
    
    private void MakeChildrenActiveByTag(String tag)
    {
        Transform transform = GameObject.FindWithTag(tag).transform;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
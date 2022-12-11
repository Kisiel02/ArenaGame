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
        PlaceWeapons();
        PlaceSpawnPoints();
        RemoveObjects();
        PlaceObjects();
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
        ReplaceObjectWithTag("SpawnWeapon");
    }

    private void PlaceSpawnPoints()
    {
        ReplaceObjectWithTag("SpawnPlayer");
    }


    private void ReplaceObjectWithTag(String tag)
    {
        Transform spawnWeapon = GameObject.FindWithTag(tag).transform;

        foreach (Transform child in spawnWeapon)
        {
            PlaceObject(child);
        }
    }

    private void PlaceObject(Transform objectToBePlaced)
    {
        bool placed = false;

        int count = 0;
        while (!placed)
        {
            if (count > 100) placed = true; //Safety check

            var newPos = RandomiseNewPosition();

            if (newPos.y >= minimalY)
            {
                objectToBePlaced.transform.position = newPos;
                placed = true;
            }

            count++;
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

    private void PlaceObjects()
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
}
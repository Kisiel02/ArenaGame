using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mesh;

    [SerializeField] private List<GameObject> objectToSpawn;

    [SerializeField] private int ObjectCount = 15;

    [SerializeField] private int maxX = 60;

    [SerializeField] private int maxZ = 60;

    [SerializeField] private int y = 30;

    [SerializeField] private float minimalY = 3.0f;

    public void RemoveObjects()
    {
        foreach (Transform child in mesh.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void GenerateObjects(int seed)
    {
        Random.InitState(seed);
        PlaceWeapons();
        RemoveObjects();
        PlaceObjects();
    }

    public void PlaceWeapons()
    {
        Transform SpawnWeapon = GameObject.FindWithTag("SpawnWeapon").transform;
        
        foreach (Transform child in SpawnWeapon)
        {
            bool placed = false;

            while (!placed)
            {
                int x = Random.Range(-maxX, maxX);
                int z = Random.Range(-maxZ, maxZ);

                Vector3 newPos = new Vector3(x, y, z);
                
                RaycastHit hit;
                if (Physics.Raycast(newPos, Vector3.down, out hit, y + 10.0f) && hit.point.y >= minimalY)
                {
                    Transform newTransform = SurfaceAligner.CalculatePositionAndAddHeight(child, 0f, true);
                    child.transform.position = newTransform.position;
                    placed = true;
                }
            }
        }

        
    }

    private void PlaceObjects()
    {
        int objectsSpawned = 0;

        while (objectsSpawned != ObjectCount)
        {
            int x = Random.Range(-maxX, maxX);
            int z = Random.Range(-maxZ, maxZ);

            GameObject choosen = objectToSpawn[Random.Range(0, objectToSpawn.Count - 1)];

            Vector3 newPos = new Vector3(x, y, z);

            RaycastHit hit;
            if (Physics.Raycast(newPos, Vector3.down, out hit, y + 10.0f) && hit.point.y >= minimalY)
            {
                var spawned = Instantiate(choosen, hit.point, Quaternion.identity);
                spawned.transform.parent = mesh.transform;
                objectsSpawned++;
            }
        }
    }
}
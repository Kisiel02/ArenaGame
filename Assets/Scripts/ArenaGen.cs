using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGen : MonoBehaviour
{
    public GameObject itemPrefab;
    public int numberOfObjects;
    public float radius = 20f;

    public void SprawnMountains() {

        itemPrefab.transform.localScale = new Vector3(30, 30, 30);
        for (int i = 0; i < numberOfObjects; i++) {

            float angle = i * Mathf.PI * 2 / numberOfObjects;
            float x = (Mathf.Cos(angle) * radius);
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);

           // float angleDegrees = -angle * Mathf.Rad2Deg;
           // Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

            Instantiate(itemPrefab, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));

        }

        
        
    }
}

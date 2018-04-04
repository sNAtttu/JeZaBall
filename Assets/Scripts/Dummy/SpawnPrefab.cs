using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public Vector3 SpawnPosition = new Vector3();
    public GameObject PrefabToSpawn;

    private GameObject spawnedObject = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(PrefabToSpawn, SpawnPosition, PrefabToSpawn.transform.rotation, null);
            }     
        }
        if (Input.GetMouseButtonDown(1))
        {
            if(spawnedObject != null)
            {
                Destroy(spawnedObject);
                spawnedObject = null;
            }
        }
    }

}

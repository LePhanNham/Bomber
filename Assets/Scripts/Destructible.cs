using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float ItemSpawnChance = 0.5f;
    public GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject,destructionTime);
    }
    private void OnDestroy()
    {
        if (spawnableItems.Length > 0 && Random.value<=ItemSpawnChance)
        {
            int randomIndex = Random.Range(0,spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex],transform.position,Quaternion.identity);
        }
    }
}

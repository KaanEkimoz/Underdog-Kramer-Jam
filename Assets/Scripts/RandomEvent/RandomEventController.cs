using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.ProBuilder.MeshOperations;

public class RandomEventController : MonoBehaviour
{
    public List<SpawnArea> spawnAreas;
    public List<Spawnable> prefabs;

    public TVController tvController;

    public float spawnInterval = 30f;
    public float timer;

    [Range(0, 100)]
    public float tvEventChance = 10f;

    void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            RandomEvent();
            timer = spawnInterval;
        }
    }

    public void RandomEvent()
    {
        /*float random = Random.Range(0, 100f);
        if(random <= tvEventChance)
        {
            if (!tvController.IsOpen())
            {
                tvController.OpenTV(); 
            }
        }
        else
        {
            SpawnRandomItem();
        }*/

    }

    public void SpawnRandomItem()
    {
        int areaNo = Random.Range(0, spawnAreas.Count);
        SpawnArea spawnArea = spawnAreas[areaNo];
        Vector3 randomPoint = spawnArea.GetRandomPoint();
        GameObject prefab = GetRandomPrefab();

        Instantiate(prefab, randomPoint, Quaternion.identity);
    }

    private GameObject GetRandomPrefab()
    {
        foreach(var p in prefabs)
        {
            int i = Random.Range(1, 101);
            if(i <= p.spawnChance)
            {
                return p.prefab;
            }
        }

        return prefabs[Random.Range(0, prefabs.Count)].prefab;
    }
}

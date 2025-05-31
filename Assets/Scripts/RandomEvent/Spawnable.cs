using UnityEngine;

[System.Serializable]
public class Spawnable
{
    public GameObject prefab;
    [Range(0, 100)]
    public float spawnChance = 100f;  
}

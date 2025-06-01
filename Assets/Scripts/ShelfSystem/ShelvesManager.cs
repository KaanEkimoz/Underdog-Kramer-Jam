using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ShelvesManager : MonoBehaviour
{
    public List<ShelfDetector> shelves = new();
    [Range(0f, 1f)] public float swapChance = 0.5f;

    private void Start()
    {
        if (shelves.Count == 0)
            shelves = Object.FindObjectsByType<ShelfDetector>(FindObjectsSortMode.None).ToList();

        ShuffleAndSwapSpawn();
    }
    public bool CheckIsAllOnCorrectPos()
    {
        foreach (var shelfDetector in shelves)
            if (!shelfDetector.HasCorrectPickupable)
                return false;

        return true;
    }
    private void ShuffleAndSwapSpawn()
    {
        // Listeyi karýþtýr
        shelves = shelves.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < shelves.Count; i += 2)
        {
            // Eðer tek raf kalmýþsa kendi itemýný spawnlasýn
            if (i + 1 >= shelves.Count)
            {
                shelves[i].SpawnStartPickupable(shelves[i].shelfItemData.shelfItemPrefab);
                continue;
            }

            var shelfA = shelves[i];
            var shelfB = shelves[i + 1];

            bool shouldSwap = Random.value < swapChance;

            if (shouldSwap)
            {
                // Yer deðiþtirerek spawn
                shelfA.SpawnStartPickupable(shelfB.shelfItemData.shelfItemPrefab);
                shelfB.SpawnStartPickupable(shelfA.shelfItemData.shelfItemPrefab);
            }
            else
            {
                // Normal spawn
                shelfA.SpawnStartPickupable(shelfA.shelfItemData.shelfItemPrefab);
                shelfB.SpawnStartPickupable(shelfB.shelfItemData.shelfItemPrefab);
            }
        }
    }

}

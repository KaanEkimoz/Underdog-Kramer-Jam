using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public string ItemName => itemData.itemName;
    public ItemCategories ItemCategory => itemData.category;
    public GameObject ItemPrefab => itemData.itemPrefab;

}

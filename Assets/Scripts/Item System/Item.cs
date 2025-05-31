using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public string ItemName => itemData.itemName;
    public ShelfItemType ItemCategory => itemData.shelfItemCategory;
    public GameObject ItemPrefab => itemData.shelfItemPrefab;

}

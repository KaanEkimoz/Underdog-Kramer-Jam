using UnityEngine;

public class ShelfItem : MonoBehaviour
{
    public ShelfItemData itemData;

    //public string ItemName => itemData.itemName;
    public ShelfItemTypes ItemCategory => itemData.shelfItemType;
    public GameObject ItemPrefab => itemData.shelfItemPrefab;

}

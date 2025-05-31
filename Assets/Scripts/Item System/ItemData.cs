using UnityEngine;

[CreateAssetMenu(fileName = "New Shelft Item", menuName = "Shelf Items")]
public class ItemData : ScriptableObject
{
    public ShelfItemType shelfItemCategory;
    public GameObject shelfItemPrefab;
}

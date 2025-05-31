using UnityEngine;

[CreateAssetMenu(fileName = "New Shelft Item", menuName = "Shelf Items")]
public class ShelfItemData : ScriptableObject
{
    public ShelfItemTypes shelfItemType;
    public GameObject shelfItemPrefab;
}

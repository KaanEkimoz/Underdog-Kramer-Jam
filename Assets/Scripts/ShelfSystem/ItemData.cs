using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemCategories category;
    public GameObject itemPrefab;
}

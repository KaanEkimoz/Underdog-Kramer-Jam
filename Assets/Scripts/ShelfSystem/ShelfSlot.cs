using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    public Item currentItem;
    public ItemCategories rightCategory;
    public bool PlaceItem(Item item)
    {
        currentItem = item;
        item.transform.position = transform.position;
        item.transform.rotation = Quaternion.identity;

        return item.itemData.category == rightCategory;           
    }

    public void RemoveItem(Item item)
    {
        currentItem = null;
    }

    public bool IsOccupied()
    {
        return currentItem != null;
    }
}

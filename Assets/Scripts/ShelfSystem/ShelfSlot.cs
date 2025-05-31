using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    public Item currentItem;
    public ItemCategories rightCategory;
    public float detectionDistance = 2f; 
    public KeyCode placeKey = KeyCode.E;

    private Camera mainCam;
    private PlayerPickup playerPickup;

    void Start()
    {
        mainCam = Camera.main;
        playerPickup = FindObjectOfType<PlayerPickup>();
    }
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward * detectionDistance);
        Gizmos.DrawRay(ray);
    }
    void Update()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance))
        {
            if (hit.collider.gameObject == gameObject) 
            {
                Debug.DrawRay(ray.origin, ray.direction * detectionDistance, Color.green);

                
                if (playerPickup.heldObject != null && Input.GetMouseButton(0))
                {
                    Item item = playerPickup.heldObject.GetComponent<Item>();
                    if (!IsOccupied() && item != null)
                    {                       
                        playerPickup.heldObject.GetComponent<Rigidbody>().useGravity = true;
                        playerPickup.heldObject.transform.SetParent(null);
                        playerPickup.pickupCamera.SetActive(false);
                        playerPickup.heldObject = null;
                     
                        bool correctCategory = PlaceItem(item);
                        Debug.Log(correctCategory ? "Doðru kategoriye yerleþtirildi." : "YANLIÞ kategori!");
                    }
                    else
                    {
                        Debug.Log("Slot dolu veya geçersiz eþya.");
                    }
                }
            }
        }
    }

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

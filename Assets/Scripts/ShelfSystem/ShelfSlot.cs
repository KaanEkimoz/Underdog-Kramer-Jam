using UnityEngine;
public class ShelfSlot : MonoBehaviour
{
    public Item currentItem;
    public ItemCategories rightCategory;
    public float detectionDistance = 2f; 
    public KeyCode placeKey = KeyCode.E;
    public bool isCorrectCategory;
    [SerializeField] private Transform placePoint;

    private Camera mainCam;
    private PlayerPickup playerPickup;
    [SerializeField] private LayerMask shelfDetectLayerMask;

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
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectionDistance, shelfDetectLayerMask ))
        {
            if (hit.collider.gameObject == gameObject) 
            {
                Debug.DrawRay(ray.origin, ray.direction * detectionDistance, Color.green);
                if (playerPickup.isDropped)
                {
                    Item item = playerPickup.lastHeldObject.GetComponent<Item>();
                    if (!IsOccupied() && item != null)
                    {
                        playerPickup.lastHeldObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;    
                        playerPickup.lastHeldObject.GetComponent<Rigidbody>().isKinematic = true;
                        PlaceItem(item);
                     
                        bool correctCategory = PlaceItem(item);
                        Debug.Log(correctCategory ? "Doðru kategoriye yerleþtirildi." : "YANLIÞ kategori!");
                    }
                    else
                    {
                        Debug.Log("Slot dolu veya geçersiz eþya.");
                    }
                    playerPickup.isDropped = false;
                }
            }
        }
    }

    public bool PlaceItem(Item item)
    {
        currentItem = item;
        item.transform.position = placePoint.position;
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

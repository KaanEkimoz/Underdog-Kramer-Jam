using UnityEngine;
public class ShelfSlot : MonoBehaviour
{
    public ShelfItem currentItem;
    public ShelfItemTypes rightCategory;
    public float detectionDistance = 2f; 
    public KeyCode placeKey = KeyCode.E;
    public bool isCorrectCategory;
    [SerializeField] private Transform placePoint;

    private PlayerPickup playerPickup;
    [SerializeField] private LayerMask shelfDetectLayerMask;

    void Start()
    {
        playerPickup = FindObjectOfType<PlayerPickup>();
    }
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * detectionDistance);
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
                    ShelfItem item = playerPickup.lastHeldObject.GetComponent<ShelfItem>();
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

    public bool PlaceItem(ShelfItem item)
    {
        currentItem = item;
        item.transform.position = placePoint.position;
        item.transform.rotation = Quaternion.identity;
        
        return item.itemData.shelfItemType == rightCategory;
    }

    public void RemoveItem(ShelfItem item)
    {
        currentItem = null;
    }

    public bool IsOccupied()
    {
        return currentItem != null;
    }
}

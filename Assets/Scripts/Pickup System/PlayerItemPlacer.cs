using UnityEngine;

[RequireComponent(typeof(PlayerPickup))]
public class PlayerItemPlacer : MonoBehaviour
{
    [SerializeField] private float maxPlaceDistance = 3f;
    private PlayerPickup pickup;

    void Start()
    {
        pickup = GetComponent<PlayerPickup>();
    }

    void Update()
    {
        
        if (pickup.heldObject != null && Input.GetMouseButtonDown(1)) 
        {
            TryPlaceHeldItem();
        }
    }

    void TryPlaceHeldItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxPlaceDistance))
        {
            ShelfSlot slot = hit.collider.GetComponent<ShelfSlot>();
            if (slot != null)
            {
                if (slot.IsOccupied())
                {
                    Debug.Log("Slot zaten dolu.");
                    return;
                }

                GameObject heldObj = pickup.heldObject;
                Item item = heldObj.GetComponent<Item>();

                bool correctCategory = slot.PlaceItem(item);

                heldObj.GetComponent<Rigidbody>().useGravity = true;
                heldObj.transform.SetParent(null);

                pickup.heldObject = null;
                
                if (pickup.pickupCamera != null)
                    pickup.pickupCamera.SetActive(false);

                if (!correctCategory)
                {
                    Debug.LogWarning("Yanlýþ kategoriye yerleþtirildi!");
                }
                else
                {
                    Debug.Log("Doðru rafa yerleþtirildi!");
                }
            }
        }
    }
}

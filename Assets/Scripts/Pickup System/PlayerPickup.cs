using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float checkSphereRadius = 0.5f;
    [SerializeField] private float maxPickupDistance = 3f;
    [SerializeField] private Transform pickupParent; // Kamera önüne objeyi konumlandýrmak için boþ bir GameObject
    [SerializeField] private LayerMask pickupLayer;

    private GameObject heldObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickup();
            else
                Drop();
        }

        if (heldObject != null)
        {
            heldObject.transform.position = pickupParent.position;
            heldObject.transform.rotation = pickupParent.rotation;
        }
    }
    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, checkSphereRadius, out RaycastHit hit, maxPickupDistance, pickupLayer))
        {
            GameObject target = hit.collider.gameObject;
            heldObject = target;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.transform.SetParent(pickupParent);
        }
    }

    void Drop()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.SetParent(null);
        heldObject = null;
    }
}

using Unity.FPS.Gameplay;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float checkSphereRadius = 0.5f;
    [SerializeField] private float maxPickupDistance = 3f;
    [SerializeField] private Transform pickupParent; // Kamera önüne objeyi konumlandýrmak için boþ bir GameObject
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private GameObject pickupCamera;
    [SerializeField] private LayerMask obstacleLayer;

    private GameObject heldObject;
    private Collider heldCollider;


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
            Vector3 targetPos = pickupParent.position;
            Quaternion targetRot = pickupParent.rotation;

            //if (!IsObstructed(targetPos, heldCollider.bounds.extents))
            //{
                heldObject.transform.position = targetPos;
                heldObject.transform.rotation = targetRot;
            //}
            

        }
    }
    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, checkSphereRadius, out RaycastHit hit, maxPickupDistance, pickupLayer))
        {
            GameObject target = hit.collider.gameObject;
            heldObject = target;
            heldCollider = heldObject.GetComponent<Collider>();
            heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.transform.SetParent(pickupParent);
            pickupCamera.SetActive(true);
        }
    }

    void Drop()
    {
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.transform.SetParent(null);
        pickupCamera.SetActive(false);
        heldObject = null;
        heldCollider = null;
    }
    bool IsObstructed(Vector3 targetPos, Vector3 extents)
    {
        // Burada objenin boyutlarýna göre çakýþma kontrolü yapýyoruz
        return Physics.CheckBox(targetPos, extents, Quaternion.identity, obstacleLayer);
    }
}

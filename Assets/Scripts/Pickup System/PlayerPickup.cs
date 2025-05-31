using System;
using UnityEngine;
public class PlayerPickup : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float checkSphereRadius = 0.5f;
    [SerializeField] private float maxPickupDistance = 3f;
    [SerializeField] private Transform pickupParent; // Kamera önüne objeyi konumlandýrmak için boþ bir GameObject
    [SerializeField] private LayerMask pickupLayer;
    public GameObject pickupCamera;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float verticalFollowStrength = 0.5f; // Yukarý aþaðý hareket gücü
    private float baseYOffset;

    public GameObject heldObject;
    public GameObject lastHeldObject;
    private Collider heldCollider;

    public bool isDropped = false;

    public Action OnItemDropped;

    void Start()
    {
        baseYOffset = pickupParent.localPosition.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickup();
            else
                Drop();
        }

        // DÜZELTÝLEN KISIM BURASI
        float camX = Camera.main.transform.eulerAngles.x;
        float verticalOffset = camX > 180 ? camX - 360 : camX; // Artýk -90 ile +90 arasý
        verticalOffset = Mathf.Clamp(verticalOffset, -60, 60); // Açý sýnýrý
        float offsetY = baseYOffset + (verticalOffset / 60f) * verticalFollowStrength;
        // SON

        Vector3 adjustedPos = pickupParent.parent.TransformPoint(new Vector3(0, offsetY, pickupParent.localPosition.z));

        if (heldObject != null)
        {
            Vector3 targetPos = pickupParent.position;
            Quaternion targetRot = pickupParent.rotation;

            heldObject.transform.position = adjustedPos;
            heldObject.transform.rotation = targetRot;
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.SphereCast(ray, checkSphereRadius, out RaycastHit hit, maxPickupDistance, pickupLayer))
        {
            GameObject target = hit.collider.gameObject;
            heldObject = target;
            heldCollider = heldObject.GetComponent<Collider>();
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.transform.SetParent(pickupParent);
            pickupCamera.SetActive(true);
            Debug.Log("Pick Upped");
        }
        Debug.Log("Ray Failed");
    }

    void Drop()
    {
        Vector3 dropPosition = heldObject.transform.position;
        Vector3 extents = heldCollider.bounds.extents;

       
        if (IsObstructed(dropPosition, extents))
        {
            float checkRadius = 1.0f;
            int checkSteps = 16; // 360 / 22.5 = 16 kontrol yönü
            bool foundSafe = false;

            for (int i = 0; i < checkSteps; i++)
            {
                float angle = i * (360f / checkSteps);
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                Vector3 testPos = dropPosition + direction * checkRadius;

                if (!IsObstructed(testPos, extents))
                {
                    heldObject.transform.position = testPos;
                    foundSafe = true;
                    break;
                }
            }
            if (!foundSafe)
            {
                Vector3 fallbackPos = dropPosition - transform.forward * 1.0f;
                heldObject.transform.position = fallbackPos;
            }
            
        }

        lastHeldObject = heldObject;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        heldObject.transform.SetParent(null);
        pickupCamera.SetActive(false);
        heldObject = null;
        heldCollider = null;
        isDropped = true;
    }

    bool IsObstructed(Vector3 targetPos, Vector3 extents)
    {
        // Burada objenin boyutlarýna göre çakýþma kontrolü yapýyoruz
        return Physics.CheckBox(targetPos, extents, Quaternion.identity, obstacleLayer);
    }
}
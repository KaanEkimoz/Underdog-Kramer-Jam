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

    [SerializeField] private float verticalFollowStrength = 0.5f; // Yukarý aþaðý hareket gücü
    private float baseYOffset;

    private GameObject heldObject;
    private Collider heldCollider;

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

            //if (!IsObstructed(targetPos, heldCollider.bounds.extents))
            //{
            heldObject.transform.position = adjustedPos;
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
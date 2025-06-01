using UnityEngine;
public class ShelfDetector : MonoBehaviour
{
    [SerializeField] private Transform snapTransform;
    [SerializeField] public ShelfItemData shelfItemData;
    //[SerializeField] private GameObject snapGameObject;
    public bool HasCorrectPickupable { get; set; }
    public bool HasPickupable { get; set; }

    private Pickupable currentPickupable;
    private Pickupable removedPickuapble;

    //Highlight
    [SerializeField] private Renderer shelfRenderer;
    [SerializeField] private Color highlightColor = Color.green;
    private Color defaultColor;
    private bool isHighlighted = false;

    private void Start()
    {
        if (shelfRenderer != null)
            defaultColor = shelfRenderer.material.color;
    }
    private void OnTriggerStay(Collider other)
    {

        if(!HasPickupable && other.TryGetComponent<Pickupable>(out currentPickupable) && !currentPickupable.IsOnShelf)
        {
            if (!isHighlighted)
                HighlightShelf(true);

            if (!currentPickupable.IsPickedUp)
            {
                PlaceToShelf(currentPickupable);
            }
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        HighlightShelf(false);
    }
    private void HighlightShelf(bool shouldHighlight)
    {
        if (shelfRenderer == null) return;

        shelfRenderer.material.color = shouldHighlight ? highlightColor : defaultColor;
        isHighlighted = shouldHighlight;
    }
    private void PlaceToShelf(Pickupable pickupable)
    {
        pickupable.PlacedToShelf(this);
        pickupable.DisablePhysics();

        HasPickupable = true;
        SnapPickupableToSurface(pickupable.gameObject, snapTransform);
        Debug.Log(pickupable.name + " placed to shelf.");
        HighlightShelf(false);
    }
    public void RemoveFromShelf(Pickupable pickupable)
    {
        currentPickupable = null;
        HasPickupable = false;
        HasCorrectPickupable = false;
        Debug.Log(pickupable.name + " removed from shelf.");
    }
    private void SnapPickupableToSurface(GameObject pickupable, Transform snapTransform)
    {
        if (pickupable.TryGetComponent<Collider>(out Collider collider))
        {
            // 1. Collider'ýn local space'teki merkezini dünya uzayýna çevir
            Vector3 localCenter = pickupable.transform.InverseTransformPoint(collider.bounds.center);

            // 2. Offset: collider'ýn alt yüzeyi ile pivot arasý mesafe
            float offsetY = localCenter.y - collider.bounds.extents.y;

            // 3. SnapTransform pozisyonuna, offset kadar yukarý taþý
            Vector3 finalPosition = snapTransform.position - new Vector3(0f, offsetY, 0f);
            pickupable.transform.position = finalPosition;

            // 4. Yönü platformun yönüyle hizala
            //pickupable.transform.rotation = Quaternion.LookRotation(snapTransform.forward, Vector3.up);
            pickupable.transform.rotation = snapTransform.rotation;
        }
        else
        {
            Debug.LogWarning($"'{pickupable.name}' has no Collider, can't snap properly.");
        }
    }
    public void SpawnStartPickupable(GameObject shelfItem)
    {
        if (shelfItemData != null)
            Instantiate(shelfItem, snapTransform.transform.position, Quaternion.identity);
    }

}

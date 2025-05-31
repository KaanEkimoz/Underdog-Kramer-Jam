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

    private void Start()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        if(!HasPickupable && other.TryGetComponent<Pickupable>(out currentPickupable) && !currentPickupable.IsPickedUp && !currentPickupable.IsOnShelf)
            PlaceToShelf(currentPickupable);
    }
    private void PlaceToShelf(Pickupable pickupable)
    {
        pickupable.PlacedToShelf(this);
        pickupable.DisablePhysics();

        HasPickupable = true;
        SnapPickupableToSurface(pickupable.gameObject, snapTransform);
        Debug.Log(pickupable.name + " placed to shelf.");
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
        if (pickupable.TryGetComponent<Renderer>(out Renderer renderer))
        {
            // 1. Objenin alt noktas�n� bul (bounds �zerinden)
            Collider objCollider = pickupable.GetComponent<Collider>();

            if (objCollider == null) return;

            // Bounds'un alt noktas�n� bul
            float objHalfY = pickupable.transform.localScale.y / 2;

            // Platformun �st y�zeyi (Y koordinat�)
            //float platformTopY = snapTransform.position.y + snapTransform.localScale.y / 2f;

            // 2. Y�kseklik fark�n� hesapla ve pozisyonu ayarla
            //float yOffset = platformTopY - objHalfY;
            //pickupable.transform.position += new Vector3(0f, yOffset, 0f);

            // 3. Objeyi platformun y�n�ne g�re d�nd�r
            // Sadece y�n� ayarlamak i�in objeyi platformun forward y�n�ne d�nd�r
            pickupable.transform.rotation = Quaternion.LookRotation(snapTransform.forward, Vector3.up);

            pickupable.transform.position = snapTransform.position;
            pickupable.transform.position += new Vector3(0, objHalfY, 0);
        }
    }
    public void SpawnStartPickupable(GameObject shelfItem)
    {
        if (shelfItemData != null)
            Instantiate(shelfItem, snapTransform.transform.position, Quaternion.identity);
    }

}

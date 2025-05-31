using UnityEngine;
public class ShelfDetector : MonoBehaviour
{
    [SerializeField] private Transform snapPosition;
    public int hasCorrectPickupable { get; set; }
    public int hasPickupable { get; set; }

    private Pickupable currentPickupable;
    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Pickupable>(out currentPickupable) && !currentPickupable.IsPickedUp)
        {
            PlaceToShelf(currentPickupable);
        }
    }
    private void PlaceToShelf(Pickupable pickupable)
    {
        pickupable.DisablePhysics();
        SnapPickupableToSurface(pickupable.gameObject, snapPosition);
    }
    private void RemoveFromShelf()
    {
        //currentPickupable.Drop();
        currentPickupable = null;
    }
    private void SnapPickupableToSurface(GameObject pickupable, Transform surface)
    {
        if (pickupable.TryGetComponent<Renderer>(out Renderer renderer))
        {
            float itemBottomY = renderer.bounds.min.y;
            float surfaceY = surface.position.y;
            float deltaY = surfaceY - itemBottomY;

            pickupable.transform.position += new Vector3(0, deltaY, 0);
        }
    }

}

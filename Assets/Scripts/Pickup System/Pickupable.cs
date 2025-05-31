using UnityEngine;
public class Pickupable : MonoBehaviour
{
    public ShelfItemData shelfItemData;
    public bool IsOnShelf { get; set; }
    public bool IsOnCorrectShelf { get; set; }
    public bool IsPickedUp { get; set; }

    private Rigidbody _rb;
    private ShelfDetector _currentShelf;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        IsPickedUp = false;
    }

    public void PickedUp()
    {
        _rb.useGravity = false;
        IsPickedUp = true;
        IsOnShelf = false;
        IsOnCorrectShelf = false;

        if (_currentShelf != null)
        {
            _currentShelf.RemoveFromShelf(this);
            _currentShelf = null;
        }
    }
    public void PlacedToShelf(ShelfDetector currentShelf)
    {
        IsPickedUp = false;
        IsOnShelf = true;
        
        _currentShelf = currentShelf;

        if (currentShelf.shelfItemType == shelfItemData.shelfItemType)
        {
            currentShelf.HasCorrectPickupable = true;
            IsOnCorrectShelf = true;
            Debug.Log(gameObject.name + " placed to CORRECT shelf.");
        }
    }
    public void Dropped()
    {
        IsPickedUp = false;
    }
    public void DisablePhysics()
    {
        _rb.isKinematic = true;
    }
    public void EnablePhysics()
    {
        _rb.isKinematic = false;
    }
}
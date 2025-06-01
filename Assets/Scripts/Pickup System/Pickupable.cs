using com.absence.soundsystem;
using com.absence.soundsystem.internals;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    //public const float COLLISION_MAGNITUDE_COEFFICIENT = 0.06f;

    [SerializeField] private SoundAsset m_soundAsset;

    public ShelfItemData shelfItemData;
    public bool IsOnShelf { get; set; }
    public bool IsOnCorrectShelf { get; set; }
    public bool IsPickedUp { get; set; }

    private Rigidbody _rb;
    private ShelfDetector _currentShelf;

    private void Awake()
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

        if (currentShelf.shelfItemData.shelfItemType == shelfItemData.shelfItemType)
        {
            currentShelf.HasCorrectPickupable = true;
            currentShelf.HasPickupable = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb.isKinematic)
            return;

        if (m_soundAsset == null)
            return;

        Sound.Create(m_soundAsset).AtPosition(collision.GetContact(0).point).Play();
    }
}
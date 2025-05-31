using UnityEngine;
public class Pickupable : MonoBehaviour
{
    public bool IsOnShelf { get; set; }
    public bool IsOnCorrectShelf { get; set; }
    public bool IsPickedUp { get; set; }

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void PickedUp()
    {
        _rb.useGravity = false;
    }

    public void Dropped()
    {
        
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
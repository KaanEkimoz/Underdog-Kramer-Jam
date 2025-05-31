using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPlayerPickup: MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private Pickupable _grabbable;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (_grabbable == null)
            {
                // Not carrying an object, try to grab
                float pickUpDistance = 4f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out _grabbable))
                    {
                        //_grabbable.PickUp(objectGrabPointTransform);
                    }
                }
            }
            else
            {
                // Currently carrying something, drop
                //_grabbable.Drop();
                _grabbable = null;
            }
        }
    }
}
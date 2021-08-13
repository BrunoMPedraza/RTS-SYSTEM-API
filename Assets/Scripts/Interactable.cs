using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractionType interactionType;

    [Header("Item Properties")]
    public ItemType itemType;
    public bool isPickedUp;

    public void OnPickUp(PlayerMotor nearby){
        nearby.holdingGold += itemType.price;
        isPickedUp = true;
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        if (itemType)
        {
            Gizmos.DrawWireSphere(transform.position, itemType.interactionType.selectionRadius);
        }
    }
}

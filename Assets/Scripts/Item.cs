using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public bool isPickedUp;

    public void onPickUp(PlayerMotor nearby){
        nearby.holdingGold += itemType.price;
        isPickedUp = true;
        //Destroy(gameObject);
    }

}

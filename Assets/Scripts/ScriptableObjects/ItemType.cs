using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item",menuName ="Item type")]
public class ItemType : ScriptableObject
{
    public InteractionType interactionType;
    public enum Type{CONSUMABLE, VALUABLE, UPGRADE}
    public Type type;
    public int price;


    public void OnValidate(){
        if(type!=Type.VALUABLE){
            price = 0;
        }
    }

}

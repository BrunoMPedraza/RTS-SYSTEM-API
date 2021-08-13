using UnityEngine;


[System.Serializable]
public class Order
{
    public bool isEmptyOrDone = false;
    #region PublicVariables
    public string orderName = "Order";
    public enum OrderType { ATTACK, FOLLOW, HARVEST, BUILD, MOVE } 
    public OrderType orderType;
    public GameObject transmitter;
    public GameObject receiver;
    public Vector3 receiverPosition;
    public float interactionRadius;
    public Order linkedOrder;

    #endregion
    public bool isCompleted() {
        switch (orderType)
        {
            case OrderType.MOVE:
                return Vector3.Distance(transmitter.transform.position, receiverPosition) <= .5f + interactionRadius;
            case OrderType.HARVEST:
                return receiver.GetComponent<Interactable>().isPickedUp;
            default:
                return false;
        }
    }

    public Order (string orderName, OrderType orderType, GameObject transmitter, Vector3 receiverPosition, GameObject receiver = null, float interactionRadius = 0, Order linkedOrder = null)
    {
        this.orderName = orderName;
        this.orderType = orderType;
        this.transmitter = transmitter;
        this.receiverPosition = receiverPosition;
        this.receiver = receiver;
        this.linkedOrder = linkedOrder;
        this.interactionRadius = interactionRadius;
    }
}

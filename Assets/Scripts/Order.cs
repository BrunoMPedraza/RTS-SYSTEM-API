using UnityEngine;


[System.Serializable]
public class Order
{
    #region PublicVariables
    public string orderName = "Order";
    public enum OrderType { ATTACK, FOLLOW, HARVEST, BUILD, MOVE } 
    public OrderType orderType = OrderType.FOLLOW;
    public GameObject emissor;
    public GameObject receiver;
    public Vector3 receiverPosition;
    public Order linkedOrder;
    #endregion
    public bool isCompleted() {
        switch (orderType)
        {
            case OrderType.MOVE:
                return Vector3.Distance(emissor.transform.position, receiverPosition) < 1f;
            case OrderType.HARVEST:
                return receiver.GetComponent<Item>().isPickedUp;
            default:
                return false;
        }
    }

    public Order (string orderName, OrderType orderType, GameObject emissor, Vector3 receiverPosition, GameObject receiver = null, Order linkedOrder = null)
    {
        this.orderName = orderName;
        this.orderType = orderType;
        this.emissor = emissor;
        this.receiverPosition = receiverPosition;
        this.receiver = receiver;
        this.linkedOrder = linkedOrder;
    }
}

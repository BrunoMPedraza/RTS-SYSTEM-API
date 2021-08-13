using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Player motor is based on the premise of orders which catch up between each other if they are completed, or end abruptly if interrumpted.
// They will also cycle to try and complete themselves in cases of "out of range"

// The order system works this way 
// #1 
[RequireComponent(typeof(NavMeshAgent), typeof(PlayerInputManager))]
public class PlayerMotor : MonoBehaviour
{
    #region Initialization
    [HideInInspector] public PlayerInputManager inputManager;
    public string playerName = "Unit";
    NavMeshAgent agent;
    public float health = 100;
    public bool isDead => health < 0;
    public int teamIndex = 0;
    public int holdingGold = 0;
    public LinkedList<Order> orders = new LinkedList<Order>();
    public Order currentOrder = null;
    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        agent = GetComponent<NavMeshAgent>();
    }
    #endregion
    #region Update
    void Update()
    {
        if(isDead || Input.GetKeyDown(PlayerInputManager.GetKeyCode("order_Stop")))
        {
            StopOrders();
        }

        if(currentOrder != null)
        {
            if(currentOrder.isCompleted())
            {
                Debug.Log("Current order completed: " + currentOrder.orderName);
                if(currentOrder.linkedOrder != null)
                    StartOrder(currentOrder.linkedOrder);
                else
                    currentOrder = null;
            }
        }
        else if(orders.Count > 0){
            StartOrder(orders.First.Value);
        }
    }
    #endregion
    public void MoveToPoint(Vector3 point) => agent.SetDestination(point);

    public void AddOrder(Order order)
    {
        orders.AddLast(order);
        Debug.Log("Order " + order.orderName + " added to " + gameObject.name);
    }
    
    public void StopOrders()
    {
        orders.Clear();
        agent.SetDestination(transform.position);
        Debug.Log("All orders stopped");
    }

    public Order FilterOrderBy(Vector3 position, Transform target)
    {
        if(target.GetComponent<PlayerMotor>())
            if(target.GetComponent<PlayerMotor>().teamIndex != teamIndex)
                return new Order("Attack " + target.name,
                                 Order.OrderType.ATTACK,
                                 gameObject,
                                 target.position,
                                 target.gameObject);
                                 
        if(target.GetComponent<Item>())
            return new Order("Harvest " + target.name,
                             Order.OrderType.HARVEST,
                             gameObject,
                             target.position,
                             target.gameObject);

        return new Order("Move towards " + position,
                         Order.OrderType.MOVE,
                         gameObject,
                         position);
    }
    
    public void StartOrder(Order order){
        Debug.Log("Order " + order.orderName);
        switch (order.orderType)
        {
            case Order.OrderType.MOVE:
                order.emissor.GetComponent<PlayerMotor>().agent.SetDestination(order.receiverPosition);
                break;
            
            case Order.OrderType.HARVEST:
                if(Vector3.Distance(order.emissor.transform.position, order.receiver.transform.position) > order.receiver.GetComponent<Item>().itemType.interactionType.selectionRadius)
                {
                    order.emissor.GetComponent<PlayerMotor>().orders.AddFirst(new Order("Move to harvest " + order.receiver, Order.OrderType.MOVE, order.emissor, order.receiver.transform.position, order.receiver, order));
                }
                order.receiver.GetComponent<Item>().onPickUp(order.emissor.GetComponent<PlayerMotor>());
                break;
        }

        order.emissor.GetComponent<PlayerMotor>().currentOrder = order.emissor.GetComponent<PlayerMotor>().orders.First.Value;
    }
}

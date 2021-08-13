using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


// Player motor is based on the premise of orders which catch up between each other if they are completed, or end abruptly if interrumpted.
// They will also cycle to try and complete themselves in cases of "out of range"

// The order system works this way 
// #1 
[RequireComponent(typeof(NavMeshAgent), typeof(PlayerInputManager), typeof(Interactable))]
public class PlayerMotor : MonoBehaviour
{
    #region Initialization
    [HideInInspector] public PlayerInputManager inputManager;
    [HideInInspector] public Interactable interactableSettings;
    [Header("Dynamic Properties")]
    public int holdingGold = 0;
    public float health = 100;
    
    [Header("Static Properties")] 
    private NavMeshAgent _agent;
    public string playerName = "Unit";
    public int teamIndex = 0;

    public List<Order> Orders;
    public LinkedList<Order> _orders = new LinkedList<Order>();
    public Order currentOrder;
    private Interactable followingTarget;
    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        interactableSettings = GetComponent<Interactable>();
        _agent = GetComponent<NavMeshAgent>();
        currentOrder.isEmptyOrDone = true;
    }
    #endregion
    
    private void Update()
    {
        Orders = _orders.ToList();
        if(IsDead || Input.GetKeyDown(PlayerInputManager.GetKeyCode("order_Stop")))
        {
            if(!currentOrder.isEmptyOrDone) StopOrders();
            return;
        }
        
        if(!currentOrder.isEmptyOrDone)
        {
            if(currentOrder.isCompleted())
            {
                Debug.Log("Current order completed: " + currentOrder.orderName);
                if (currentOrder.linkedOrder != null)
                {
                    Debug.Log(currentOrder.orderName + " finished. Starting order " + currentOrder.linkedOrder.orderName);
                    currentOrder = currentOrder.linkedOrder;
                    StartOrder(currentOrder);
                }

                currentOrder.isEmptyOrDone = true;
                followingTarget = null;
                _orders.RemoveFirst();
            }

            if (followingTarget)
            {
                MoveAndLockOnto(followingTarget);
            }
        }
    }
    
    private bool IsDead => health <= 0;

    private void MoveToPoint(Vector3 point, float interactionRadius = 0)
    {
        _agent.stoppingDistance = interactionRadius - 0.5f;
        _agent.SetDestination(point);
    }
    
    private void MoveAndLockOnto(Interactable followingTarget)
    {
        this.followingTarget = followingTarget;
        _agent.stoppingDistance = followingTarget.interactionType.selectionRadius - 0.5f;
        _agent.SetDestination(this.followingTarget.transform.position);
    }

    public void AddOrder(Order order)
    {
        if (IsDead)
        {
            Debug.Log("Can't add order since " + playerName + " is dead.");
            return;
        }
        if(order.linkedOrder == null) _orders.Clear();
        _orders.AddFirst(order);
        currentOrder = _orders.First();
        Debug.Log("Order " + order.orderName + " added to " + gameObject.name);
        StartOrder(currentOrder);
    }
    
    public void StopOrders()
    {
        _orders.Clear();
        currentOrder.isEmptyOrDone = true;
        _agent.SetDestination(transform.position);
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
            else
                return new Order("Follow " + target.name,
                    Order.OrderType.FOLLOW,
                    gameObject,
                    target.position,
                    target.gameObject, interactableSettings.interactionType.selectionRadius);
                                 
        if(target.GetComponent<Interactable>())
            if(target.GetComponent<Interactable>().itemType)
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
        Debug.Log("Order " + order.orderName + " started.");
        switch (order.orderType)
        {
            case Order.OrderType.MOVE:
                order.transmitter.GetComponent<PlayerMotor>().MoveToPoint(order.receiverPosition, order.interactionRadius);
                break;
            
            case Order.OrderType.HARVEST:
                if(Vector3.Distance(order.transmitter.transform.position, order.receiver.transform.position) > 1f + order.receiver.GetComponent<Interactable>().interactionType.selectionRadius)
                {
                    order.transmitter.GetComponent<PlayerMotor>().AddOrder(new Order("Move to harvest " + order.receiver, Order.OrderType.MOVE, order.transmitter, order.receiver.transform.position, order.receiver, order.receiver.GetComponent<Interactable>().interactionType.selectionRadius, order));
                    Debug.Log("Order " + order.orderName + " waiting for move order to finish....");
                    return;
                }
                order.receiver.GetComponent<Interactable>().OnPickUp(order.transmitter.GetComponent<PlayerMotor>());
                break;
            
            case Order.OrderType.FOLLOW:
                order.transmitter.GetComponent<PlayerMotor>().MoveAndLockOnto(order.receiver.GetComponent<Interactable>());
                break;
        }
    }
}

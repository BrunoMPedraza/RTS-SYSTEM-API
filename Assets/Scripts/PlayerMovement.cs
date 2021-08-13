using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerMovement : MonoBehaviour
{
    #region Initialization 
    Camera cam;
    PlayerMotor motor;
    Vector3 mouseTarget;
    
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }
    #endregion
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            motor.AddOrder(motor.FilterOrderBy(PositionOnMouse, TransformOnMouse));
            Debug.Log("Added order to " + gameObject.name);
        }
    }

    public Vector3 PositionOnMouse
    {
        get
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }

    public Transform TransformOnMouse
    {
        get
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform;
            }
            return null;
        }
    }
}

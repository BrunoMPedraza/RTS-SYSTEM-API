using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerMovement : MonoBehaviour
{
    Camera cam;
    public LayerMask movementMask;
    PlayerMotor motor;
    Vector3 mouseTarget;

    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            motor.AddOrder(motor.FilterOrderBy(GetPositionOnMouse(), GetTransformOnMouse()));
            Debug.Log("Added order to " + gameObject.name);
        }
    }


    Vector3 GetPositionOnMouse(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit)){
            return hit.point;
        }
        return Vector3.zero;
    }

    Transform GetTransformOnMouse(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) { 
            return hit.transform;
        }
        return null;
    }
}

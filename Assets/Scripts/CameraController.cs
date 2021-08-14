using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool keyboardControl;
    public float cameraSpeed = 5f;
    public float panSpeed = 5f;
    public float panBorderThickness = 10f;
    public Vector3 panPosition;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minY = -20f;
    public float maxY = 45f;
    public float defaultZoom = 19.6f;
    public bool rememberZoom = false;
    public bool allowZoom = true;

    #region CameraLock stuff
    public Transform lockTarget;
    public Vector3 offSet;
    public bool isLocked = true;
    #endregion


    private void Awake()
    {
        defaultZoom = transform.position.y;
        panPosition = transform.position - offSet;
        offSet.z *= -1;
        keyboardControl = PlayerInputManager.currentKeyHandler.keyboardControlsCamera;
        // panPosition.y = defaultZoom;
    }

    void Update()
    {
        #region zoomLimits
        //Remember zoom should not be changed on execution
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(allowZoom){
            if (rememberZoom)
            { 
                panPosition.y = panPosition.y<minY ? minY :
                panPosition.y>maxY ? maxY : 
                panPosition.y-= scroll * scrollSpeed * 1000f * Time.deltaTime;
            }
            else // This works, but peraphs we can do better.
            {
                offSet.y = offSet.y<Math.Abs(minY) ? Math.Abs(minY) :
                offSet.y>maxY*2 ? maxY*2 : 
                offSet.y-= scroll * scrollSpeed * 1000f * Time.deltaTime; 
                //TODO for some reason it jiggles when you force the wheel. Maybe disable the upward value?
            }
        }
        #endregion
        #region cameraHold
        //This checks "Is camera being held? Then disable non conditional camera behavior"
        if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("camera_lockHold_key"))){
            isLocked = !isLocked;
        }
        if(isLocked){
            offSet.z = offSet.y/2;
            panPosition = lockTarget.position;
            LookAtPosition(lockTarget.position);
    
            
        } 
        else{
        #endregion
        #region updateCamera (Non-conditional)
        panPosition.z += 
        (Input.mousePosition.y >= Screen.height - 
        panBorderThickness ? 1 : Input.mousePosition.y <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Vertical") : 0) * panSpeed * Time.deltaTime);

        panPosition.x += 
        (Input.mousePosition.x >= Screen.width - 
        panBorderThickness ? 1 : Input.mousePosition.x <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Horizontal") : 0) * panSpeed * Time.deltaTime);

 
        panPosition.x = Mathf.Clamp(panPosition.x, -panLimit.x, panLimit.x);
        panPosition.y = Mathf.Clamp(panPosition.y, minY, maxY);
        panPosition.z = Mathf.Clamp(panPosition.z, -panLimit.y, panLimit.y);
        LookAtPosition(panPosition);
        }
        #endregion
    }

    public void LookAtPosition(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, CalculateDesiredPosition(target), cameraSpeed * Time.deltaTime);
    }

    public Vector3 CalculateDesiredPosition(Vector3 target){ //CALLBACK
        return new Vector3(target.x, target.y + offSet.y, target.z - offSet.z);;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(panPosition, Vector3.one);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x, panPosition.y, transform.position.z), panPosition);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(transform.position.x, panPosition.y, transform.position.z), transform.position);

        if(lockTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(lockTarget.position, lockTarget.localScale);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(transform.position.x, lockTarget.position.y, transform.position.z), lockTarget.position);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(transform.position.x, lockTarget.position.y, transform.position.z), transform.position);
        }
    }



}
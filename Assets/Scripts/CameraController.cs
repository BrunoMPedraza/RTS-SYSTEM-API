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
        // defaultZoom = transform.position.y;
        panPosition = transform.position - offSet;
        // offSet.z *= -1;
        // keyboardControl = PlayerInputManager.currentKeyHandler.keyboardControlsCamera;
        // panPosition.y = defaultZoom;
    }

    void Update()
    {
        #region zoomBehavior
        //Remember zoom should not be changed on execution
        // float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(allowZoom){
        //     offSet.y = 
        //     offSet.y < Math.Abs(minY) ? Math.Abs(minY) :
        //     offSet.y > maxY ? maxY :
        //     (!rememberZoom) ? offSet.y -= scroll * scrollSpeed * 1000f * Time.deltaTime :
        //     (!isLocked) ?  offSet.y -= scroll * scrollSpeed * 1000f * Time.deltaTime : defaultZoom;
        // }
        // #endregion
        // #region cameraHold
        // //This checks "Is camera being held? Then disable non conditional camera behavior"
        // if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("camera_lockHold_key")))
        //     isLocked = !isLocked;

        // if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("camera_restoreZoom_key")))
        //     RestoreZoomCamera();

        // if(isLocked){
        //     offSet.z = offSet.y/2;
        //     if(rememberZoom) RestoreZoomCamera();
        //     panPosition = lockTarget.position;
        //     LookAtPosition(lockTarget.position);
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

        //panPosition.y = CalculateVerticalPosition(panPosition, LayerMask.GetMask("Ground"));
 
        panPosition.x = Mathf.Clamp(panPosition.x, -panLimit.x, panLimit.x);
        panPosition.z = Mathf.Clamp(panPosition.z, -panLimit.y, panLimit.y);
        LookAtPosition(panPosition);
        }
        #endregion
        
    }

    /*public float CalculateVerticalPosition(Vector3 xzPosition, LayerMask layerMask)
    {

        Ray ray = new Ray(xzPosition + (Vector3.up * 10), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask.value))
        {
            return (int)hit.point.y;
        }
        return 0;
    }
    */

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

    public void RestoreZoomCamera(){
        offSet.y = defaultZoom;
    }

}
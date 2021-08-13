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
    public float minY = 0;
    public float maxY = 75f;

    #region CameraLock stuff
    public Transform lockTarget;
    public Vector3 offSet;
    public bool isLocked = true;
    #endregion


    private void Awake()
    {
        panPosition = transform.position - offSet;
        offSet.z *= -1;
        keyboardControl = PlayerInputManager.currentKeyHandler.keyboardControlsCamera;
    }

    void Update()
    {
        #region cameraHold
        if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("camera_lockHold_key"))){
            isLocked = !isLocked;
        }
        if(isLocked){
            panPosition = lockTarget.position;
            LookAtPosition(lockTarget.position);
        }
        else{
            #endregion

        panPosition.z += 
        (Input.mousePosition.y >= Screen.height - 
        panBorderThickness ? 1 : Input.mousePosition.y <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Vertical") : 0) * panSpeed * Time.deltaTime);

        panPosition.x += 
        (Input.mousePosition.x >= Screen.width - 
        panBorderThickness ? 1 : Input.mousePosition.x <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Horizontal") : 0) * panSpeed * Time.deltaTime);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        panPosition.y -= scroll * scrollSpeed * 1000f * Time.deltaTime;

        panPosition.x = Mathf.Clamp(panPosition.x, -panLimit.x, panLimit.x);
        panPosition.y = Mathf.Clamp(panPosition.y, minY, maxY);
        panPosition.z = Mathf.Clamp(panPosition.z, -panLimit.y, panLimit.y);
        LookAtPosition(panPosition);
        }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHandlerController : MonoBehaviour
{
    #region Boolean Options
    [Header("Optional features")]
    public bool hasArrowMovement = true;
    public bool hasRotation = true; 
    public bool hasCameraSpeedUp = true; [Tooltip("By default shift key")]
    public bool hasZoom = true;
    public bool hasAnchor = true;
    public bool hasMouseRotation = true;
    #endregion
    public Transform focusTransform = null;
    public Transform cameraTransform;
    private bool _isLocked;
    [Header("Speed Settings")]    
    public float cameraMovementSpeed = 1f;
    private float _cameraFastMovementSpeed = 2f;
    [Tooltip("How much does the camera speed up?.")]
    public float cameraFastMultiplier = 4f; 
    private float _normalSpeed = 1f;
    [Tooltip("How fast the camera freezes after it stops moving?")]
    public float cameraFriction = 8f; 
    public Vector3 newPosition;
    public Quaternion newRotation;
    public float rotationAmount;
    [Header("Zoom Settings")]
    public bool MouseWheelZoom = true;
    public bool KeyboardZoom = true;
    public float zoomStrength = 5;
    private Vector3 _zoomPower;
    private Vector3 _newZoom;

    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;
    private Vector3 _rotateStartPosition;
    private Vector3 _rotateCurrentPosition;
    [Header("Mouse displacement Settings")]
    public bool hasMouseMovement = true;
    public float cameraBorderThickness = 6f;
    void Start()
    {
        _normalSpeed = cameraMovementSpeed;
        _cameraFastMovementSpeed = cameraMovementSpeed * cameraFastMultiplier;
        newPosition = transform.position;
        newRotation = transform.rotation;
        _newZoom = cameraTransform.localPosition;
        _zoomPower = new Vector3(0, -zoomStrength/5, zoomStrength/5);
    }


    // Update is called once per frame
    void Update()
    {
        if(focusTransform!=null){
            transform.position = focusTransform.position;
        }else{
            HandleInput();
        }
    }

    public void RestoreZoomCamera(){
        // offSet.y = defaultZoom;
    }
    void HandleInput()
    {
        //CALLBACK HELL
        if(hasAnchor)HandleAnchorMovement();
        if(hasZoom)HandleCameraZoom();
        if(hasCameraSpeedUp)HandleFastSpeed();
        if(hasArrowMovement)HandleArrowKeyMovement();
        if(hasMouseMovement)HandleMouseMovement();
        if(hasRotation)HandleRotation();

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraFriction);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * cameraFriction);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * cameraFriction);
    }
    void HandleAnchorMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }

        if (hasMouseRotation){
            if(Input.GetMouseButtonDown(1)){
                _rotateStartPosition = Input.mousePosition;
            }
            if(Input.GetMouseButton(1)){
                _rotateCurrentPosition = Input.mousePosition;
                Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;
                _rotateStartPosition = _rotateCurrentPosition;
                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }
    }
    void HandleCameraZoom(){
        if(KeyboardZoom){
            _newZoom =
            (Input.GetKey(PlayerInputManager.GetKeyCode("camera_zoomIn"))) ? _newZoom + _zoomPower/3f :
            (Input.GetKey(PlayerInputManager.GetKeyCode("camera_zoomOut"))) ? _newZoom - _zoomPower/3f : _newZoom;
        }

        if(MouseWheelZoom){
            _newZoom =
            (Input.mouseScrollDelta.y != 0) ? _newZoom + (Input.mouseScrollDelta.y * (_zoomPower*2)) : _newZoom;
        }
        
    }

    void HandleFastSpeed(){
        cameraMovementSpeed = 
        (Input.GetKey(PlayerInputManager.GetKeyCode("camera_activateFastSpeed"))) ? 
        _cameraFastMovementSpeed : _normalSpeed;
    }

    void HandleRotation(){
        if (Input.GetKey(PlayerInputManager.GetKeyCode("camera_rotateRight_key")))
        newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        if (Input.GetKey(PlayerInputManager.GetKeyCode("camera_rotateLeft_key")))
        newRotation *= Quaternion.Euler(Vector3.down * rotationAmount);
    }
    void HandleArrowKeyMovement(){
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveForward_key")))
            newPosition += (transform.forward * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveBack_key")))
            newPosition -= (transform.forward * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveRight_key")))
            newPosition += (transform.right * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveLeft_key")))
            newPosition -= (transform.right * cameraMovementSpeed);
    }

    void HandleMouseMovement(){
        if(Input.mousePosition.y>=Screen.height-cameraBorderThickness){
            newPosition += (transform.forward * cameraMovementSpeed);}
        if(Input.mousePosition.y<=cameraBorderThickness){
            newPosition -= (transform.forward * cameraMovementSpeed);}
        if(Input.mousePosition.x>=Screen.width-cameraBorderThickness){
            newPosition += (transform.right * cameraMovementSpeed);}
        if(Input.mousePosition.x<=cameraBorderThickness){
            newPosition -= (transform.right * cameraMovementSpeed);}
    }
}

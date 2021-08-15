using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHandlerController : MonoBehaviour
{
    [Header("Features")]

    [Tooltip("Follow someone?")]    
    public Transform focusTransform = null;
    [Tooltip("Main camera")]    
    public Transform cameraTransform;
    private bool _isLocked;
    [Header("Movement Settings")]    
    public bool hasCameraSpeedUp = true; [Tooltip("By default shift key")]
    public bool hasArrowMovement = true;
    public float cameraMovementSpeed = 1f;
    private float _cameraFastMovementSpeed = 2f;
    [Tooltip("How much does the camera speed up?.")]
    public float cameraFastMultiplier = 4f; 
    private float _normalSpeed = 1f;
    [Tooltip("How fast the camera freezes after it stops moving?")]
    public float cameraFriction = 8f; 
    public bool hasAnchor = true;
    private Vector3 _newPosition;
    [Header("Rotation settings")]
    private Quaternion _newRotation;
    public bool hasRotation = true; 
    public float rotationAmount;
    [Header("Zoom Settings")]
    public bool hasZoom = true;
    public bool MouseWheelZoom = true;
    public bool KeyboardZoom = true;
    public float zoomStrength = 5;
    public float zoomDownLimit = 5f;
    public float zoomUpLimit = 45f;
    private Vector3 _zoomPower;
    private Vector3 _newZoom;
    private Vector3 _defaultZoom;
    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;
    private Vector3 _rotateStartPosition;
    private Vector3 _rotateCurrentPosition;
    [Header("Mouse displacement Settings")]
    public bool hasMouseMovement = true;
    public bool hasMouseRotation = true;
    private float _cameraBorderThickness = 6f;
    void Start()
    {
        _normalSpeed = cameraMovementSpeed;
        _cameraFastMovementSpeed = cameraMovementSpeed * cameraFastMultiplier;
        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = cameraTransform.localPosition;
        _defaultZoom.y = _newZoom.y;
        _zoomPower = new Vector3(0, -zoomStrength/5, zoomStrength/5);
    }


    // Update is called once per frame
    void Update()
    {
        HandleInputCanFocus();
        if(focusTransform!=null){
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * cameraFriction);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * cameraFriction);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * cameraFriction);
            _newPosition.x = focusTransform.position.x;
            _newPosition.z = focusTransform.position.z;
        }else{
            HandleInput();
        }
    }

    void HandleInputCanFocus(){
        if(hasZoom)HandleCameraZoom();
    }
    void HandleInput()
    {
        //CALLBACK HELL
        if(hasRotation)HandleRotation();
        if(hasAnchor)HandleAnchorMovement();
        if(hasCameraSpeedUp)HandleFastSpeed();
        if(hasArrowMovement)HandleArrowKeyMovement();
        if(hasMouseMovement)HandleMouseMovement();

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * cameraFriction);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * cameraFriction);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newZoom, Time.deltaTime * cameraFriction);
    }
    void HandleAnchorMovement()
    {
        //TODO: the cursor should become an anchor and be limited to a small moving, if not it will activate border displacement.
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);
                _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }

        if (hasMouseRotation){
            if(Input.GetMouseButtonDown(2)){
                Cursor.lockState = CursorLockMode.Locked;
                _rotateStartPosition = Input.mousePosition;
            }
            if(Input.GetMouseButton(2)){
                Cursor.lockState = CursorLockMode.Locked;
                _rotateCurrentPosition = Input.mousePosition;
                Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;
                _rotateStartPosition = _rotateCurrentPosition;
                _newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }
    }
    void HandleCameraZoom(){
        if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("camera_restoreZoom")))
            _newZoom.y = _defaultZoom.y;

        if(KeyboardZoom){
            _newZoom =
            (Input.GetKey(PlayerInputManager.GetKeyCode("camera_zoomIn"))) ? _newZoom + _zoomPower/3f :
            (Input.GetKey(PlayerInputManager.GetKeyCode("camera_zoomOut"))) ? _newZoom - _zoomPower/3f : _newZoom;
        }

        if(MouseWheelZoom){
            _newZoom =
            (Input.mouseScrollDelta.y != 0) ? _newZoom + (Input.mouseScrollDelta.y * (_zoomPower*2)) : _newZoom;
        }

        _newZoom.y =
            _newZoom.y < zoomDownLimit ? zoomDownLimit :
            _newZoom.y > zoomUpLimit ? zoomUpLimit :
            _newZoom.y;
        // TODO: Why does this keeps moving after full zoom? Probably the problem is here
    }

    void HandleFastSpeed(){
        cameraMovementSpeed = 
        (Input.GetKey(PlayerInputManager.GetKeyCode("camera_activateFastSpeed"))) ? 
        _cameraFastMovementSpeed : _normalSpeed;
    }

    void HandleRotation(){
        if (Input.GetKey(PlayerInputManager.GetKeyCode("camera_rotateRight_key")))
        _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        if (Input.GetKey(PlayerInputManager.GetKeyCode("camera_rotateLeft_key")))
        _newRotation *= Quaternion.Euler(Vector3.down * rotationAmount);
    }
    void HandleArrowKeyMovement(){
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveForward_key")))
            _newPosition += (transform.forward * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveBack_key")))
            _newPosition -= (transform.forward * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveRight_key")))
            _newPosition += (transform.right * cameraMovementSpeed);
        if(Input.GetKey(PlayerInputManager.GetKeyCode("camera_moveLeft_key")))
            _newPosition -= (transform.right * cameraMovementSpeed);
    }

    void HandleMouseMovement(){
        if(Input.mousePosition.y>=Screen.height-_cameraBorderThickness){
            _newPosition += (transform.forward * cameraMovementSpeed);}
        if(Input.mousePosition.y<=_cameraBorderThickness){
            _newPosition -= (transform.forward * cameraMovementSpeed);}
        if(Input.mousePosition.x>=Screen.width-_cameraBorderThickness){
            _newPosition += (transform.right * cameraMovementSpeed);}
        if(Input.mousePosition.x<=_cameraBorderThickness){
            _newPosition -= (transform.right * cameraMovementSpeed);}
    }

    
}

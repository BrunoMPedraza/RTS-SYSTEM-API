using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerMovement : MonoBehaviour
{
    #region Initialization

    private Camera _cam;
    private PlayerMotor _motor;
    
    void Awake()
    {
        if (!_cam) _cam = FindObjectOfType<Camera>();
        _motor = GetComponent<PlayerMotor>();
    }
    
    #endregion
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            _motor.AddOrder(_motor.FilterOrderBy(PositionOnMouse, TransformOnMouse));
        }
    }

    public Vector3 PositionOnMouse
    {
        get
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
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
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform;
            }
            return null;
        }
    }
}

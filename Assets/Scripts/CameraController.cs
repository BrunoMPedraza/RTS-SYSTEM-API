using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public bool keyboardControl = false;
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    public float minY = 5f;
    public float maxY = 75f;
    public Vector3 offsetFromTarget;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && player)
            transform.position = player.position + offsetFromTarget;

        var pos = transform.position;
        pos.z += 
        (Input.mousePosition.y >= Screen.height - 
        panBorderThickness ? 1 : Input.mousePosition.y <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Vertical") : 0) * panSpeed * Time.deltaTime);

        pos.x += 
        (Input.mousePosition.x >= Screen.width - 
        panBorderThickness ? 1 : Input.mousePosition.x <= 
        panBorderThickness ? -1 : 0 + (keyboardControl ? Input.GetAxisRaw("Horizontal") : 0) * panSpeed * Time.deltaTime);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 1000f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
        transform.position = pos;
    }
}

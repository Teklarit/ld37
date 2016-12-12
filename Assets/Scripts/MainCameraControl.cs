using UnityEngine;
using System.Collections;

public class MainCameraControl : MonoBehaviour
{
    //public GameObject look_camera;
    public UiMessage ui_message;
    public IngameMenuFns ingame_menu;
    public aimController aim_controller;

    public float mouseSensitivity = 5.0f;        // Mouse rotation sensitivity.
    public float speed = 10.0f;    // Regular speed.
    public float gravity = 20.0f;    // Gravity force.
    public float shiftAdd = 25.0f;    // Multiplied by how long shift is held.  Basically running.
    public float maxShift = 100.0f;    // Maximum speed when holding shift.
    public bool walkerMode = false;    // Walker Mode.

    public float zoom_speed = 0.05f;
    public float normal_fov = 60.0f;
    public float zoomed_fov = 30.0f;

    public LayerMask interactibleLayerMask;

    private float totalRun = 1.0f;
    private float rotationY = 0.0f;
    private float maximumY = 90.0f;    // Not recommended to change
    private float minimumY = -90.0f;    // these parameters.
	[SerializeField]
    private CharacterController controller;

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // Toggle mode.
            walkerMode = !walkerMode;
        }
        /*if (Input.GetMouseButton(0))
        {
            ui_message.show_message(@"Not a penny. I have been content, sir, you should lay my countenance to pawn; I have grated upon my good friends for three reprieves for you and your coach-fellow Nym; or else you had looked through the grate, like a geminy of baboons. I am damned in hell for swearing to gentlemen my friends, you were good soldiers and tall fellows; and when Mistress Bridget lost the handle of her fan, I took't upon mine honour thou hadst it not.");
        }*/

    }

    void LateUpdate()
    {
        if (ingame_menu.is_active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            return;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // FOV
        var cam = gameObject.GetComponent<Camera>();
        if (Input.GetButton("Fire2"))
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomed_fov, zoom_speed);
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal_fov, zoom_speed);

        // Mouse commands.
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);

        // Keyboard commands.
        Vector3 p = getDirection();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1.0f, 1000.0f);
            p = p * speed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (walkerMode)
        {
            // Walker Mode.
            p = transform.TransformDirection(p);
            p.y = 0.0f;
            p.y -= gravity * Time.deltaTime;
            controller.Move(p);
        }
        else
        {
            // Fly Mode.
            if (Input.GetButton("Jump"))
            { // If player wants to move on X and Z axis only (sliding)
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }
        }

        // raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 8.0f, interactibleLayerMask))
        {
            aim_controller.is_ineractable = true;
            Collider hit_collider = hit.collider;
            print("Get collider, game_object: " + hit_collider.gameObject);
            print("Found an object - distance: " + hit.distance);
        }
        else
        {
            aim_controller.is_ineractable = false;
        }
    }

    private Vector3 getDirection()
    {
        Vector3 p_Velocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        // Strifing enabled only in Fly Mode.
        if (!walkerMode)
        {
            if (Input.GetKey(KeyCode.F))
            {
                p_Velocity += new Vector3(0.0f, -1.0f, 0.0f);
            }
            if (Input.GetKey(KeyCode.R))
            {
                p_Velocity += new Vector3(0.0f, 1.0f, 0.0f);
            }
        }
        return p_Velocity;
    }

    public void resetRotation(Vector3 lookAt)
    {
        transform.LookAt(lookAt);
    }
}
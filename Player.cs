using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float sensitivity;
    public float playerHeight;
    public float groundDrag;
    public float jumpForce;
    public LayerMask whatIsGround;

    Rigidbody rb;
    Camera cam;
    Vector3 moveDir;
    GameObject[] tools;
    GameObject currentTool;
    float xRotation, yRotation;
    bool grounded;
    int currentToolId = -1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = gameObject.GetComponent<Rigidbody>();
        cam = Camera.main;
        xRotation = yRotation = 0.0f;
        rb.freezeRotation = true;

        tools = new GameObject[cam.transform.GetChild(0).childCount];
        for (int i = 0; i < tools.Length; i++) tools[i] = cam.transform.GetChild(0).GetChild(i).gameObject;
        currentTool = tools[0];
    }

    void Update()
    {
        look();
        movement();
        selectTool();
    }

    void selectTool() // 0-> nothing, 1-> scanner
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(currentToolId != 1)
            {
                currentToolId = 1;
                currentTool.SetActive(false);
                tools[currentToolId].SetActive(true);
                currentTool = tools[currentToolId];
            }
            else
            {
                currentTool.SetActive(false);
                currentToolId = 0;
                currentTool = tools[currentToolId];
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentToolId != 2)
            {
                currentToolId = 2;
                currentTool.SetActive(false);
                tools[currentToolId].SetActive(true);
                currentTool = tools[currentToolId];
            }
            else
            {
                currentTool.SetActive(false);
                currentToolId = 0;
                currentTool = tools[currentToolId];
            }
        }
    }

    void look()
    {
        yRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        xRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime *  sensitivity;
        cam.transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
    }

    void movement()
    {
        grounded = Physics.Raycast(gameObject.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else rb.drag = 0;

        if(Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal") * Time.deltaTime, verticalInput = Input.GetAxisRaw("Vertical") * Time.deltaTime;
        moveDir = cam.transform.forward * verticalInput + cam.transform.right * horizontalInput;

        rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 newVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
        }
    }
}

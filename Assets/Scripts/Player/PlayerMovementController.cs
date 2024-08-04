using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpHeight = 1f;
    public float gravity = 9.81f;
    public float airControl = 5f;
    public float sprintSpeedMultiplier = 1.5f;
    
    private CharacterController controller;
    private Vector3 input;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUI();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);

        input = ((transform.right * moveHorizontal) + (transform.forward * moveVertical)).normalized;

        input *= moveSpeed;

        if (sprint) input *= sprintSpeedMultiplier;

        if (controller.isGrounded)
        {
            moveDirection = input;
            
            // We can jump
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f; 
            }
        }
        else
        {
            // We are midair
            input.y = moveDirection.y;

            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        // Let gravity affect it. Time is not squared because Move() multiplies with deltaTime
        moveDirection.y -= (gravity * Time.deltaTime);
        
        controller.Move(moveDirection * Time.deltaTime);
    }

    void HandleUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShopUIController.Toggle();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUIController.Toggle();
        }
    }

    void HandleKiosk()
    {
        KioskUIController.Toggle();
    }
}

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

    private float normalHeight;
    public float teabagSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        normalHeight = transform.localScale.y;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKey(KeyCode.T))
        {
            StartTeabagging();
        }
        else
        {
            StopTeabagging();
        }
    }

    void StartTeabagging()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            new Vector3(1, 0.5f, 1),
            Time.deltaTime * teabagSpeed
        );
        Vector3 newPosition = new Vector3(
            transform.position.x,
            0.5f,
            transform.position.z
        );
        transform.position = Vector3.Lerp(
            transform.position,
            newPosition,
            Time.deltaTime * teabagSpeed
        );
    }

    void StopTeabagging()
    {
        if (transform.localScale.y == normalHeight) return;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            new Vector3(1, normalHeight, 1),
            Time.deltaTime * teabagSpeed
        );
        Vector3 newPosition = new Vector3(
            transform.position.x,
            normalHeight,
            transform.position.z
        );
        transform.position = Vector3.Lerp(
            transform.position,
            newPosition,
            Time.deltaTime * teabagSpeed
        );
    }
}

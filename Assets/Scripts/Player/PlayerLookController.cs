using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
	public float mouseSensitivity = 200f;
    
	private Transform playerBody;
	private float pitch;
    private bool isShopping = false;
    
	// Start is called before the first frame update
	void Start()
	{
		playerBody = transform.parent.transform;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
	{
        if (isShopping) return;

		float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
		// Yaw: Rotate around Y axis for looking left/right
		playerBody.Rotate(Vector3.up * moveX);
        
		// Pitch: Rotate around X axis for looking up/down
		pitch -= moveY;
		pitch = Mathf.Clamp(pitch, -90f, 90f);
		transform.localRotation = Quaternion.Euler(pitch, 0, 0);
	}

    public void SetIsShopping(bool isShopping)
    {
        this.isShopping = isShopping;
    }
}
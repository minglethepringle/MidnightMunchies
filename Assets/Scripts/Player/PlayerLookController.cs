using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
	public static bool locked = false;
	
	public float mouseSensitivity = 200f;
    
	private Transform playerBody;
	private float pitch;
    
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
        if (locked) return;

		float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
		// Yaw: Rotate around Y axis for looking left/right
		playerBody.Rotate(Vector3.up * moveX);
        
		// Pitch: Rotate around X axis for looking up/down
		pitch -= moveY;
		pitch = Mathf.Clamp(pitch, -90f, 90f);
		transform.localRotation = Quaternion.Euler(pitch, 0, 0);
	}

	public static void HideWeapons()
	{
		// Move it down and hide it
		GameObject.FindGameObjectWithTag("Weapon").transform.localPosition -= new Vector3(0, 1000, 0);
		GameObject.FindGameObjectWithTag("Crosshair").GetComponent<CanvasRenderer>().cull = true;
	}
	
	public static void ShowWeapons()
	{
		GameObject.FindGameObjectWithTag("Weapon").transform.localPosition += new Vector3(0, 1000, 0);
		GameObject.FindGameObjectWithTag("Crosshair").GetComponent<CanvasRenderer>().cull = false;
	}
}
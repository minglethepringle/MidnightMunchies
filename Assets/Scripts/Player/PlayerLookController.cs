using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
	public static bool locked = false;
	
	public static float mouseSensitivity = 200f;

	public static float volume = 1.0f;
    
	private Transform playerBody;
	private float pitch;
	private static GameObject activeWeapon;
    
	// Start is called before the first frame update
	void Start()
	{
		playerBody = transform.parent.transform;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		activeWeapon = GameObject.FindGameObjectWithTag("Weapon");
	}

	// Update is called once per frame
	void Update()
	{
        if (locked) return;
		if (!PauseMenuBehavior.isGamePaused)
        {
			float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

			// Yaw: Rotate around Y axis for looking left/right
			playerBody.Rotate(Vector3.up * moveX);

			// Pitch: Rotate around X axis for looking up/down
			pitch -= moveY;
			pitch = Mathf.Clamp(pitch, -90f, 90f);
			transform.localRotation = Quaternion.Euler(pitch, 0, 0);
		}
	}

	public static void HideWeapons()
	{
		activeWeapon = GameObject.FindGameObjectWithTag("Weapon");
		if (activeWeapon)
			activeWeapon.SetActive(false);
		GameObject.FindGameObjectWithTag("Crosshair").GetComponent<CanvasRenderer>().cull = true;
	}
	
	public static void ShowWeapons()
	{
		if (activeWeapon)
			activeWeapon.SetActive(true);
		GameObject.FindGameObjectWithTag("Crosshair").GetComponent<CanvasRenderer>().cull = false;
	}

	public static void SetMouseSens(float value)
    {
		mouseSensitivity = value;
    }

	public static void SetVolume(float value)
    {
		AudioListener.volume = value;
	}
}
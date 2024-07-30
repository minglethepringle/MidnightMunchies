using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SojaExiles

{
	public class opencloseDoor : MonoBehaviour
	{

		public Animator openandclose;
		public bool open;
		public Transform Player;
		public Text hintText;

		void Start()
		{
			open = false;
		}

		private void Update()
		{
			if (Player)
			{
				// If you're looking at the door and closer than 5 units, show the hint text
				float dist = Vector3.Distance(Player.position, transform.position);
				if (dist < 5)
				{
					hintText.gameObject.SetActive(true);
					// If the door is closed and you press F, open the door
					if (open == false)
					{
						if (Input.GetKeyDown(Controls.OPEN_DOOR))
						{
							StartCoroutine(opening());
						}
					}
					// If the door is open and you press F, close the door
					else
					{
						if (open == true)
						{
							if (Input.GetKeyDown(Controls.OPEN_DOOR))
							{
								StartCoroutine(closing());
							}
						}
					}
				}
				else
				{
					hintText.gameObject.SetActive(false);
				}
			}
		}

		IEnumerator opening()
		{
			print("you are opening the door");
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			print("you are closing the door");
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}
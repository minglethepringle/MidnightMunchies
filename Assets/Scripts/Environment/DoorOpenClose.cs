using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorOpenClose : MonoBehaviour
{
    public Transform player;
    public Text hintText;
    public GameObject otherDoor;
    public bool isLeftDoor;
    
    private Animator animator;
    public bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    private void OnMouseOver()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < 5)
        {
            hintText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isOpen)
                {
                    StartCoroutine(CloseDoor());
                    if (otherDoor)
                    {
                        StartCoroutine(otherDoor.GetComponent<DoorOpenClose>().CloseDoor());
                    }
                }
                else
                {
                    StartCoroutine(OpenDoor());
                    if (otherDoor)
                    {
                        StartCoroutine(otherDoor.GetComponent<DoorOpenClose>().OpenDoor());
                    }
                }
            }
        }
    }

    private void OnMouseExit()
    {
        hintText.gameObject.SetActive(false);
    }
    
    public IEnumerator OpenDoor()
    {
        animator.Play(isLeftDoor ? "Opening" : "Opening 1");

        isOpen = true;
        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator CloseDoor()
    {
        animator.Play(isLeftDoor ? "Closing" : "Closing 1");
        isOpen = false;
        yield return new WaitForSeconds(.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyBehavior : MonoBehaviour
{
    public enum NPCState
    {
        IDLE,
        INTERACTING
    };
    
    public NPCState state;
    
    public Text hintText;
    public Transform player;
    
    private bool isLookingAtDummy;
    private Quaternion targetRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        state = NPCState.IDLE;
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Controls.NPC_INTERACT) && isLookingAtDummy)
        {
            state = NPCState.INTERACTING;
            StartCoroutine(SpeakHint());
        }
        
        // Always face player but don't rotate upwards or downwards
        targetRotation = Quaternion.LookRotation(player.position - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private IEnumerator SpeakHint()
    {
        string originalText = hintText.text;
        hintText.text = "\"Place an order on the kiosk to complete the round!\"";
        hintText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(3);
        
        hintText.gameObject.SetActive(false);
        hintText.text = originalText;
        state = NPCState.IDLE;
    }

    private void OnMouseOver()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < 5)
        {
            hintText.gameObject.SetActive(true);
            isLookingAtDummy = true;
        }
        else
        {
            hintText.gameObject.SetActive(false);
            isLookingAtDummy = false;
        }
    }
    
    private void OnMouseExit()
    {
        hintText.gameObject.SetActive(false);
        isLookingAtDummy = false;
    }
}

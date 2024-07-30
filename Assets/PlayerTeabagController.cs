using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeabagController : MonoBehaviour
{
    public float teabagSpeed = 10f;

    private float originalY;
    
    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Controls.TEABAG))
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                new Vector3(transform.localPosition.x, 0.5f * originalY, transform.localPosition.z),
                Time.deltaTime * teabagSpeed
            );
        }
        else
        {
            if (transform.localPosition.y == originalY) return;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                new Vector3(transform.localPosition.x, originalY, transform.localPosition.z),
                Time.deltaTime * teabagSpeed
            );
        }
    }
}

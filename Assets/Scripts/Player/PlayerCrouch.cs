using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=33hSsPpoET4

public class PlayerCrouch : MonoBehaviour
{
    private bool isCrouching = false;
    public bool IsCrouching => isCrouching; // Getter for isCrouching

    [SerializeField] private float crouchHeight = .5f;  // The height we shrink to when crouching
    private Vector2 normalHeight;

    void Start()
    {
        normalHeight = transform.localScale;
    }

    void Update()
    {
        if (Input.GetButton("Crouch") && GetComponent<PlayerMovement>().IsGrounded) // If we are crouching and grounded
        {
            isCrouching = true;
            
            if (transform.localScale.y != crouchHeight)
            {
                transform.localScale = new Vector2(normalHeight.x, crouchHeight);
            }
            
        }
        else
        {
            isCrouching = false;
            if (transform.localScale.y != normalHeight.y)
            {
                transform.localScale = normalHeight;
            }
        }
    }
}

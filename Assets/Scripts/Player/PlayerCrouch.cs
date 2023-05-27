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
    private Vector2 normalSpriteHeight;
    private Vector3 normalSpritePosition;

    [SerializeField] GameObject playerSprite;  // We need this to cancel out the squish on it so the animation looks normal


    void Start()
    {
        normalHeight = transform.localScale;
        normalSpriteHeight = playerSprite.transform.localScale;
    }

    void Update()
    {
        if (Input.GetButton("Crouch") && GetComponent<PlayerMovement>().IsGrounded) // If we are crouching and grounded
        {
            isCrouching = true;
            
            if (transform.localScale.y != crouchHeight)
            {
                transform.localScale = new Vector2(normalHeight.x, crouchHeight);
                playerSprite.transform.localScale = new Vector2(normalSpriteHeight.x, normalSpriteHeight.y * 2);
                playerSprite.transform.localPosition = new Vector3(normalSpritePosition.x, normalSpritePosition.y + normalSpritePosition.y, normalSpritePosition.z); // = normalSpritePosition;
            }
            
        }
        else
        {
            isCrouching = false;
            if (transform.localScale.y != normalHeight.y)
            {
                transform.localScale = normalHeight;
                playerSprite.transform.localScale = normalSpriteHeight;
                playerSprite.transform.localPosition = normalSpritePosition;
            }
        }

        if (!isCrouching)
        {
            normalSpritePosition = playerSprite.transform.localPosition;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=33hSsPpoET4

public class PlayerCrouch : MonoBehaviour
{
    private bool isCrouching = false;
    public bool IsCrouching => isCrouching; // Getter for isCrouching

    [SerializeField] private float crouchHeight = .5f;  // The height we shrink to when crouching
    private float direction = 1; // To keep the direction we face when crouching consistent with the way we were facing before
    private Vector2 normalHeight;
    private Vector2 normalSpriteHeight;
    private Vector3 normalSpritePosition;

    [SerializeField] GameObject playerSprite;  // We need this to cancel out the squish on it so the animation looks normal
    private Animator anim;


    void Start()
    {
        normalHeight = transform.localScale;
        normalSpriteHeight = playerSprite.transform.localScale;
        anim = playerSprite.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Crouch") && GetComponent<PlayerMovement>().IsGrounded && !GetComponent<PlayerMovement>().Disabled) // If we are crouching and grounded
        {
            isCrouching = true;
            if (!GetComponent<PlayerMovement>().RightFacing)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            
            if (transform.localScale.y != crouchHeight)
            {
                transform.localScale = new Vector2(normalHeight.x * direction, crouchHeight);
                playerSprite.transform.localScale = new Vector2(normalSpriteHeight.x, normalSpriteHeight.y * 2);
                playerSprite.transform.localPosition = new Vector3(normalSpritePosition.x, normalSpritePosition.y + normalSpritePosition.y, normalSpritePosition.z); // = normalSpritePosition;
            }
            
        }
        else
        {
            isCrouching = false;
            if (transform.localScale.y != normalHeight.y)
            {
                transform.localScale = new Vector2(normalHeight.x * direction, normalHeight.y);
                playerSprite.transform.localScale = normalSpriteHeight;
                playerSprite.transform.localPosition = normalSpritePosition;
                StartCoroutine(DoUncrouch());
            }
        }

        if (!isCrouching)
        {
            normalSpritePosition = playerSprite.transform.localPosition;
        }

        anim.SetBool("isCrouching", isCrouching);

    }

    IEnumerator DoUncrouch()
    {
        anim.Play("CrouchUp", 0, 0);
        yield return new WaitForSeconds(.2f);
        anim.Play("Idle");
        yield return null;
    }
}

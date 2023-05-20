using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject slashbox;

    private Collider2D slashCol;
    private SpriteRenderer slashRend;
    private Animator anim;

    bool activeCoroutine;
    bool slash1Active;  // Booleans that let us know which slash is active
    bool slash2Active;
    bool slash3Active;

    void Start()
    {
        slashCol = slashbox.GetComponent<Collider2D>();      //Get slashbox collider component
        slashRend = slashbox.GetComponent<SpriteRenderer>(); //Get slashbox sprite renderer Component
        anim = GetComponent<Animator>();            //Get Animator Component

        slashCol.enabled = false;

        // Have another script on the slashbox that deals with what happens when someone is within the collider
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerCrouch>().IsCrouching)  // We do nothing when we crouch
        {
            if (Input.GetButtonDown("Slash"))
            {
                StartCoroutine(DoSlash1());
            }
            else if (!activeCoroutine)
            {
                slashCol.enabled = false;
                slashRend.color = Color.clear;
            }
        }
    }

    IEnumerator DoSlash1()
    {
        activeCoroutine = true;
        slash1Active = true;

        Debug.Log("Slash!!");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        yield return new WaitForSeconds(.1f);

        activeCoroutine = false;
        slash1Active = true;
        yield return null;
    }
}

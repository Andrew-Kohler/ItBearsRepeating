using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    public bool canSprint;          // Whether we can sprint according to the meter
    public float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float lossRate;    // How fast we lose and gain stamina
    [SerializeField] private float gainRate;
    private PlayerMovement move;    // Movement, so we can tell it if we can sprint/air dash

    private bool activeCoroutine;
    void Start()
    {
        move = GetComponent<PlayerMovement>();
        canSprint = true;
        activeCoroutine = false;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeCoroutine)   // If we aren't running, say, a regen coroutine
        {  
            if (canSprint)  // If we are CAPABLE of sprinting
            {
                // If we are sprinting
                if (move.IsSprinting)
                {
                    stamina -= (lossRate * Time.deltaTime);
                    if (move.IsAirDashing)
                    {
                        StartCoroutine(DoAirDash());
                    }
                }
                // Else if we are not sprinting
                else if (!move.IsSprinting && stamina < maxStamina)
                {
                    stamina += (gainRate * Time.deltaTime);
                }

                // If stamina is less than or equal to 0
                if (stamina <= 0)
                {
                    StartCoroutine(DoStaminaRegen());
                }
            }           
        }
    } // End of update

    // Coroutines -------------------------------------------
    IEnumerator DoAirDash()
    {
        activeCoroutine = true;
        stamina -= (.5f * maxStamina);
        if(stamina < 0)
        {
            stamina = 0;
        }
        yield return new WaitUntil(()=>move.IsGrounded);
        activeCoroutine = false;
        yield return null;
    }
    IEnumerator DoStaminaRegen()    // If the player burns all of their stamina, they are forced to let it recharge
    {
        canSprint = false;          // No running right now, we're getting it all back
        activeCoroutine = true;

        while (stamina < maxStamina)
        {
            stamina += (gainRate * Time.deltaTime);
            yield return new WaitForSeconds(.001f);
        }

        canSprint = true;
        activeCoroutine = false;
        yield return null;
    }
}

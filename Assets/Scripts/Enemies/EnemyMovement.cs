using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyType { Sasha, Melee, Laser, Rocket };    // Determines movement behavior
    [Header("Movement Variables")]
    [SerializeField] EnemyType enemyType = EnemyType.Melee;
    //This is whether or not the enemy is allowed to move
    [SerializeField] private bool disabled = false;
    // The speed at which the enemy moves
    [SerializeField] private float Speed = 2.7f;
    // Acceleration of the enemy. Smaller number = faster
    [SerializeField] private float Acceleration = .05f;
    // This means that you're 1D character is facing left by default (1D means you only face left or right)")]
    [SerializeField] private bool SpriteFacingRight = true;

    [Header("Jump Variables")]
    //Controls whether your player can jump or not
    [SerializeField] private bool canJump = true;
    //The force of your jump (Be sure to have your gravity set to 1 for side-scroller
    [SerializeField] private float JumpForce = 7f;
    //Number of jumps the enemy can do each time they touch the ground. (2 = Double jump)
    [SerializeField] private int NumberOfJumps = 1;
    // The multiplier at which you fall down (used for smooth movement) and it can't be below 1
    [SerializeField] private float FallMultiplier = 3f;


    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Find the Rigidbody component on the gameobject this script is attached to.
        col = GetComponent<Collider2D>(); //Get Collider component
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Public methods ----------------------------------------

    public void TakeKnockback(Vector3 knockback)
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    // Private methods ----------------------------------------
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); //Stop any previous vertical movement
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); //Add force upwards as an impulse.
        //if (playerAudio && playerAudio != null)
        //{
        //  playerAudio.JumpSource.Play();
        //}
    }

    // Coroutines ----------------------------------------

    // So...knockback. Add a force in the opposite direction they're facing?
    // I could actually pass knockback as a vector to have more control over the angle + proportion
    // Ughhh, that sounds so awesome, let's do that
    // Sweet, did that
}

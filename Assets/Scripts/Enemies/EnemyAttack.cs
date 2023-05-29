using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject meleeHitbox;    // If a melee enemy, we need a hitbox
    [SerializeField] GameObject laserPrefab;    // If a laser enemy, we need the laser prefab
    [SerializeField] GameObject rocketPrefab;    // If a rocket enemy, we need the laser prefab
    [SerializeField] GameObject enemySprite;    // Actual enemy sprite for anims

    private Collider2D meleeCol;    // If we're melee, we need the collider on the hitbox
    private Animator anim;          // Enemy anim
    private EnemyMovement move;     // Enemy movement

    // DMG is the damage it does to the bear, SPD is how fast the attack can be performed
    [Header("Melee Stats")]
    [SerializeField] private float meleeDMG = 1f;
    [SerializeField] private float meleeSPD = 1f;
    [SerializeField] private Vector3 meleeKnockback = new Vector3(0f, 0f, 0f);
    [Header("Laser Stats")]
    [SerializeField] private float laserDMG = .3f;
    [SerializeField] private float laserSPD = .5f;
    [SerializeField] private float laserDuration = 7f;
    [SerializeField] private Vector3 laserKnockback = new Vector3(0f, 0f, 0f);
    [Header("Rocket Stats")]
    [SerializeField] private float rocketDMG = 10f;
    [SerializeField] private float rocketSPD = .5f;
    [SerializeField] private float rocketDuration = 7f;
    [SerializeField] private Vector3 rocketKnockback = new Vector3(5f, 5f, 0f);

    private float currentDMG;           // These are the values that we can change internally in this class and then pass along to other classes that need them
    private Vector3 currentKnockback;
    public float DMG => currentDMG;
    public Vector3 Knockback => currentKnockback;

    private bool activeCoroutine;

    void Start()
    {
        anim = enemySprite.GetComponent<Animator>();            //Get Animator component
        move = GetComponent<EnemyMovement>();                   // Get Movement component

        if(move.typeOfEnemy == EnemyMovement.EnemyType.Melee)
        {
            meleeCol = meleeHitbox.GetComponent<Collider2D>();
            meleeCol.enabled = false;
        }       

    }

    // Update is called once per frame
    void Update()
    {
        if (move.isAttack && !activeCoroutine && !move.Disabled)  // If now is the time to attack and we aren't currently attacking
        {
            if(move.typeOfEnemy == EnemyMovement.EnemyType.Melee)
            {
                StartCoroutine(DoMeleeAttack());
            }
            else if (move.typeOfEnemy == EnemyMovement.EnemyType.Laser)
            {
                StartCoroutine(DoLaserAttack());
            }
            else if (move.typeOfEnemy == EnemyMovement.EnemyType.Rocket)
            {
                StartCoroutine(DoRocketAttack());
            }
        }
        else
        {
            if (!move.isAttack || move.Disabled)
            {
                StopAllCoroutines();
                activeCoroutine = false;
                if(move.typeOfEnemy == EnemyMovement.EnemyType.Melee)
                {
                    meleeCol.enabled = false;
                }
            }
            
        }
    }

    // Public methods ---------------------------------------------------------

    // Private methods ---------------------------------------------------------

    // Coroutines ---------------------------------------------------------

    IEnumerator DoMeleeAttack()
    {
        activeCoroutine = true;
        // Same deal as player attack, enabling the hitbox and playing the animation and all that
        currentDMG = meleeDMG;
        currentKnockback = meleeKnockback;

        meleeCol.enabled = true;
        yield return new WaitForSeconds(.1f);
        meleeCol.enabled = false;
        yield return new WaitForSeconds(.8f);

        activeCoroutine = false;
        yield return null;
    }

    IEnumerator DoLaserAttack()
    {
        activeCoroutine = true;

        // This will be a little different, we'll be spawning the projectile and letting it fly, but I think I get it
        GameObject laser  = Instantiate(laserPrefab, this.transform.position, Quaternion.identity);
        laser.GetComponent<Projectile>().SetValues(laserDuration, laserDMG, laserKnockback);
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        Vector2 direction = Vector2.right;
        if (!move.RightFacing)
            direction *= -1;
        rb.AddForce(direction * laserSPD, ForceMode2D.Impulse);

        yield return new WaitForSeconds(2f);



        activeCoroutine = false;
        yield return null;
    }

    IEnumerator DoRocketAttack()
    {
        activeCoroutine = true;

        // This, though...oof. Finally, I'll have to figure out how to calculate a launch arc between two points
        GameObject rocket = Instantiate(rocketPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 45)));
        rocket.GetComponent<Projectile>().SetValues(rocketDuration, rocketDMG, rocketKnockback);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();

        Vector2 direction = new Vector2(1, 1);
        if (!move.RightFacing)
            direction.x *= -1;
        rb.AddForce(direction * rocketSPD, ForceMode2D.Impulse);

        yield return new WaitForSeconds(3f);

        activeCoroutine = false;
        yield return null;
    }
}

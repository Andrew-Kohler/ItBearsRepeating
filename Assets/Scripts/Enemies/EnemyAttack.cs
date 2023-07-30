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
    [SerializeField] GameObject bear;           // For distance calc on the rocket

    private Collider2D meleeCol;    // If we're melee, we need the collider on the hitbox
    private Animator anim;          // Enemy anim
    private EnemyMovement move;     // Enemy movement
    private AudioSource audioS;

    // DMG is the damage it does to the bear, SPD is how fast the attack can be performed
    [Header("Melee Stats")]
    [SerializeField] private float meleeDMG = 1f;
    [SerializeField] private float meleeSPD = 1f;
    [SerializeField] private Vector3 meleeKnockback = new Vector3(0f, 0f, 0f);
    [SerializeField] private AudioClip meleeSound1;
    
    [Header("Laser Stats")]
    [SerializeField] private float laserDMG = .3f;
    [SerializeField] private float laserProjSPD = 5f;  // How fast the laser travels
    [SerializeField] private float laserAtkSPD = 2f;   // How frequently the bot fires
    [SerializeField] private float laserDuration = 7f;
    [SerializeField] private Vector3 laserKnockback = new Vector3(0f, 0f, 0f);
    [SerializeField] private AudioClip laserSound1;

    [Header("Rocket Stats")]
    [SerializeField] private float rocketDMG = 10f;
    [SerializeField] private float rocketSPD = .5f;
    [SerializeField] private float rocketDuration = 7f;
    [SerializeField] private Vector3 rocketKnockback = new Vector3(5f, 5f, 0f);
    [SerializeField] private AudioClip rocketSound1;
    [SerializeField] private AudioClip rocketSound2;
    [SerializeField] private AudioClip rocketSound3;
    private float rocketXRef = 21f; // The value that approximates the maximum x range of the rocket

    private float currentDMG;           // These are the values that we can change internally in this class and then pass along to other classes that need them
    private Vector3 currentKnockback;
    public float DMG => currentDMG;
    public Vector3 Knockback => currentKnockback;

    private bool activeCoroutine;

    void Start()
    {
        anim = enemySprite.GetComponent<Animator>();            //Get Animator component
        move = GetComponent<EnemyMovement>();                   // Get Movement component
        audioS = GetComponent<AudioSource>();

        //if(move.typeOfEnemy == EnemyMovement.EnemyType.Melee)
        //{
        meleeCol = meleeHitbox.GetComponent<Collider2D>();
            meleeCol.enabled = false;
        //}       

    }

    // Update is called once per frame
    void Update()
    {
        if (move.isAttack && !activeCoroutine && !move.Disabled && !GameManager.Instance.isGameOver())  // If now is the time to attack and we aren't currently attacking
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
            if (!move.isAttack || move.Disabled || GameManager.Instance.isGameOver())
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

    public void DamagePlayerMelee(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerTakeDamage>().TakeDamage(this.gameObject);
    }

    // Private methods ---------------------------------------------------------

    private void playRocketSound()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            audioS.PlayOneShot(rocketSound1, .3f);
        }
        else if (rand == 1)
        {
            audioS.PlayOneShot(rocketSound2, .3f);
        }
        else
        {
            audioS.PlayOneShot(rocketSound3, .3f);
        }
    }

    // Coroutines ---------------------------------------------------------

    IEnumerator DoMeleeAttack()
    {
        activeCoroutine = true;
        // Same deal as player attack, enabling the hitbox and playing the animation and all that
        currentDMG = meleeDMG;
        currentKnockback = meleeKnockback;

        anim.Play("Attack", 0, 0);
        yield return new WaitForSeconds(.332f);
        meleeCol.enabled = true;
        audioS.PlayOneShot(meleeSound1, .4f);
        yield return new WaitForSeconds(.2f);
        meleeCol.enabled = false;
        yield return new WaitForSeconds(meleeSPD - .332f);

        activeCoroutine = false;
        yield return null;
    }

    IEnumerator DoLaserAttack()
    {
        activeCoroutine = true;

        // This will be a little different, we'll be spawning the projectile and letting it fly, but I think I get it
        anim.Play("Attack", 0, 0);
        audioS.PlayOneShot(laserSound1, .3f);
        GameObject laser  = Instantiate(laserPrefab, this.transform.position, Quaternion.identity);
        laser.GetComponent<Projectile>().SetValues(laserDuration, laserDMG, laserKnockback, true);
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        Vector2 direction = Vector2.right;
        if (!move.RightFacing)
            direction *= -1;
        rb.AddForce(direction * laserProjSPD, ForceMode2D.Impulse);

        yield return new WaitForSeconds(laserAtkSPD);

        activeCoroutine = false;
        yield return null;
    }

    IEnumerator DoRocketAttack()
    {
        activeCoroutine = true;
        Vector2 direction = new Vector2(1, 1);
        Vector3 tempRocketKnockback = rocketKnockback;
        float angle = 45f;
        float tempRocketSpeed = rocketSPD;
        bool right = true;
        if (Mathf.Abs(transform.position.x - bear.transform.position.x) < rocketXRef)
        {
            
            direction = new Vector2((rocketXRef - (rocketXRef - Mathf.Abs(transform.position.x - bear.transform.position.x))) / rocketXRef, (rocketXRef + (rocketXRef - Mathf.Abs(transform.position.x - bear.transform.position.x))) / rocketXRef);
            tempRocketSpeed = rocketSPD - ((rocketSPD - ((Mathf.Abs(transform.position.x - bear.transform.position.x) / rocketXRef) * (rocketSPD))) / 2);
            angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            //Debug.Log(angle);
            //Debug.Log((Mathf.Abs(transform.position.x - bear.transform.position.x)) / rocketXRef);
        }

        if (!move.RightFacing)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.y);
            direction.x *= -1;
            angle += 90;
            tempRocketKnockback.x = tempRocketKnockback.x * -1;
            right = false;
        }

        //Debug.Log(angle);
        anim.Play("Attack", 0, 0);
        playRocketSound();
        GameObject rocket = Instantiate(rocketPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        rocket.GetComponent<Projectile>().SetValues(rocketDuration, rocketDMG, tempRocketKnockback, right);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();

       
            
        rb.AddForce(direction * tempRocketSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(3f);

        activeCoroutine = false;
        yield return null;
    }
}

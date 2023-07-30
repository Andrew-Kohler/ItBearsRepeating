using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTakeDamage : MonoBehaviour
{
    Health objectHealth;
    DamageFlash damageFlash;
    Rigidbody2D rb;
    AudioSource audioS;

    [SerializeField] PointOfInterestCam treePoint;
    [SerializeField] AudioClip treeFall;
    [SerializeField] AudioClip treeHit1;
    [SerializeField] AudioClip treeHit2;

    bool fallen;

    // Start is called before the first frame update
    void Start()
    {
        objectHealth = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        audioS = GetComponent<AudioSource>();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Topple()
    {
        StartCoroutine(DoTopple());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Hitbox") && !fallen)   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            // try get component on PlayerAttack or ItemAttack, get the appropriate damage and knock nums from those
            if (collision.gameObject.GetComponentInParent<PlayerAttack>() != null && !fallen)
            {
                /*int rand = Random.Range(0, 2);
                if(rand == 0)
                {
                    audioS.PlayOneShot(treeHit1);
                }
                else
                {*/
                    audioS.PlayOneShot(treeHit2);
                //}
                objectHealth.TakeDamage(collision.gameObject.GetComponentInParent<PlayerAttack>().DMG);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player Hitbox") && !fallen)   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            if (collision.gameObject.GetComponent<SmackableAttack>() != null && !fallen)
            {
                /*int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    audioS.PlayOneShot(treeHit1);
                }
                else
                {*/
                    audioS.PlayOneShot(treeHit2);
                //}
                objectHealth.TakeDamage(collision.gameObject.GetComponent<SmackableAttack>().DMG);
            }
        }
    }

    IEnumerator DoTopple()
    {
        rb.WakeUp();    // Wake up the object, add the force, and turn off its health
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(Vector2.right * 30f, ForceMode2D.Impulse);
        objectHealth.enabled = false;
        fallen = true;
        this.tag = "Terrain";       // Change its type to terrain so the player can walk on it
        this.gameObject.layer = 6;

        // Disable the player and zoom the camera out so they can watch it fall
        GameObject.Find("Bear").GetComponent<PlayerMovement>().DisablePlayer(true);
        GameObject.Find("Bear").GetComponent<PlayerMovement>().hitstun = true;
        treePoint.CallZoomOut();
        audioS.PlayOneShot(treeFall);

        // Wait a predetermined amount of time, then de-zoom the camera and reenable the player
        yield return new WaitForSeconds(5f);
        treePoint.CallZoomIn();

        GameObject.Find("Bear").GetComponent<PlayerMovement>().DisablePlayer(false);
        GameObject.Find("Bear").GetComponent<PlayerMovement>().hitstun = false;

        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return null;
    }
}

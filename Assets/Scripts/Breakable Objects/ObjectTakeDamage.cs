using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectTakeDamage : MonoBehaviour
{
    Health objectHealth;
    DamageFlash damageFlash;
    AudioSource audioS;
    [SerializeField] AudioClip shipBlowUp;

    // Start is called before the first frame update
    void Start()
    {
        objectHealth = GetComponent<Health>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        audioS = GetComponent<AudioSource>();
    }

    public void BlowUpTimeMachine()
    {
        StartCoroutine(DoBlowUpTimeMachine());
    }

    IEnumerator DoBlowUpTimeMachine()
    {
        ViewManager.Show<LevelStartView>(false);
        ViewManager.GetView<LevelStartView>().SwapColors();
        audioS.PlayOneShot(shipBlowUp, 1f);
        ViewManager.GetView<LevelStartView>().EndLevel(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
        //Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Hitbox"))   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            audioS.Play();
            // try get component on PlayerAttack or ItemAttack, get the appropriate damage and knock nums from those
            if (collision.gameObject.GetComponentInParent<PlayerAttack>() != null)
            {
                objectHealth.TakeDamage(collision.gameObject.GetComponentInParent<PlayerAttack>().DMG);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player Hitbox"))   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            if (collision.gameObject.GetComponent<SmackableAttack>() != null)
            {
                objectHealth.TakeDamage(collision.gameObject.GetComponent<SmackableAttack>().DMG);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBerryBush : MonoBehaviour
{
    // Ok, so we're gonna need... 
    // A trigger, so a circle collider
    private CircleCollider2D trigger;
    // The sprite
    private SpriteRenderer berries;
    // The player
    public GameObject player;
    // Whether or not the player pressing "E" does anything
    private bool canInteract = false;
    // A message about the ability to interact with this thing
    [SerializeField] private GameObject interactPrompt;
    private AudioSource audioS;
    void Start()
    {
        trigger = GetComponent<CircleCollider2D>();
        berries = GetComponentInChildren<SpriteRenderer>();
        audioS = GetComponent<AudioSource>();
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetButtonDown("Interact"))
            {
                ViewManager.GetView<InGameUIView>().ActivateHealthWheel();
                StartCoroutine(DoEndTutorial());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            interactPrompt.SetActive(true);
        }
    }

    IEnumerator DoEndTutorial()
    {
        interactPrompt.SetActive(false);
        canInteract = false;
        //berries.gameObject.SetActive(false);
        audioS.Play();
        yield return new WaitForSeconds(1f);
        //Destroy(this.gameObject);
    }
}

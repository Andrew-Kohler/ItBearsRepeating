using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoStarter : MonoBehaviour
{
    [SerializeField] private GameObject firstSpeaker;
    private DialogueManager dialogueManager;
    private BoxCollider2D boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        dialogueManager = firstSpeaker.GetComponent<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        firstSpeaker.SetActive(true);
        dialogueManager.ReadNextLine();
        this.gameObject.SetActive(false);
    }
}

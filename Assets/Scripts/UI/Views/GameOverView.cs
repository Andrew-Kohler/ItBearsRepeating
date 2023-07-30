using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverView : View
{
    [SerializeField] private GameObject player;   // The actual player sprite, and the overlay used to keep the sprite on screen
    [SerializeField] private GameObject spriteOverlay;

    [SerializeField] private TextMeshProUGUI gameOverText;  // The text that types out "game over"
    [SerializeField] private GameObject screenContents;     // The rest of the contents of the screen
    private string over = "Game Over";

    [SerializeField] private AudioClip keySound1;
    [SerializeField] private AudioClip keySound2;
    [SerializeField] private AudioClip keySound3;
    private AudioSource audioS;

    public override void Initialize()
    {
        audioS = GetComponent<AudioSource>();
        CanvasGroup canvasGroup = screenContents.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        GetComponent<CanvasGroup>().alpha = 0;
    }

    private void OnEnable() // On awake, match the bear sprite to the player position
    {
        spriteOverlay.transform.position = player.transform.position;
        StartCoroutine(DoGameOverReveal());
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Ok, so:
    // Player dies
    // Correctly position the bear based on the position of the player
    // We fade out the in-game UI, and fade in the (blank) game over screen
    // Once it's faded in, type out "Game Over"
    // Once that's done, fade in the other stuff

    IEnumerator DoGameOverReveal()
    {
        gameOverText.text = "";
        CanvasGroup canvasGroup = screenContents.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        yield return new WaitForSeconds(3f);

        // Type out the text letter by letter 
        var numCharsRevealed = 0;
        while (numCharsRevealed < over.Length)
        {
            ++numCharsRevealed;
            if (numCharsRevealed % 3 == 0)// As each letter reads out, do a keyboard SFX
            {
                audioS.PlayOneShot(keySound1);
            }
            else if (numCharsRevealed % 4 == 0)
            {
                audioS.PlayOneShot(keySound2);
            }
            else
            {
                audioS.PlayOneShot(keySound3);
            }
            gameOverText.text = over.Substring(0, numCharsRevealed);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(2f); // Hold for a few moments

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        yield return null;
    }

}

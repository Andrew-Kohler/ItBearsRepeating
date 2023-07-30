using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelStartView : View
{
    [SerializeField] private string levelStartText; // The date at the start of each level
    [SerializeField] private bool whiteScreen; // Whether the start screen is white or black
    [SerializeField] private bool transition;   // Whether we transition on fadeout
    [SerializeField] private float textSpeed = .3f;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image panel;
    [SerializeField] private View transitionTo; // Whatever view we're transitioning to

    private bool intro;
    [SerializeField] private AudioClip keySound1;
    [SerializeField] private AudioClip keySound2;
    [SerializeField] private AudioClip keySound3;
    private AudioSource audioS;

    public override void Initialize()
    {
        if (whiteScreen)
        {
            text.color = Color.black;
            panel.color = Color.white;
        }
        intro = false;
        GameManager.Instance.Gameplay(false);
        audioS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //text.text = "";
        
    }
    void Update()
    {
        if (!intro)
        {
            intro = true;
            StartCoroutine(DoLevelStart());
        }
    }

    public void EndLevel(int nextIndex)
    {
        text.text = "";
        GetComponent<FadeUI>().UIFadeIn(nextIndex);
    }


    IEnumerator DoLevelStart()
    {
        text.text = "";
        yield return new WaitForSeconds(2f);

        // Type out the text letter by letter 
        var numCharsRevealed = 0;
        while (numCharsRevealed < levelStartText.Length)
        { 
            ++numCharsRevealed;
            if(numCharsRevealed % 3 == 0)// As each letter reads out, do a keyboard SFX
            {
                audioS.PlayOneShot(keySound1);
            }
            else if(numCharsRevealed % 4 == 0)
            {
                audioS.PlayOneShot(keySound2);
            }
            else
            {
                audioS.PlayOneShot(keySound3);
            }
            text.text = levelStartText.Substring(0, numCharsRevealed);

            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(3f); // Hold for a few moments

        if (transition)
        {
            GetComponent<FadeUI>().UIFadeOut(transitionTo);
        }
        else
        {
            GetComponent<FadeUI>().UIFadeOut();
        }
        


        // Fade the screen out to reveal the level
        yield return null;
    }

    
}

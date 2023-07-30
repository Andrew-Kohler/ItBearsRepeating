using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Text Boxes + Setup [Do Not Change]")]
    [SerializeField] private TextMeshPro speakerNameText;   // The TMPro assets holding the text
    [SerializeField] private TextMeshPro speakerWords;
    [SerializeField] private SpriteRenderer mainBox;        // Main box (for sprite purposes)

    [Header("Speaker Details")]
    [SerializeField] private string speakerName;    // Name of the speaker
    [SerializeField] private bool isFacingRight;    // Whether the speaker is facing left or right
    [SerializeField] private bool startActivated;   // Is a conversation activated by scene start or player distance?
    [SerializeField] private bool distanceActivated;
    [SerializeField] private float activationDistanceX;
    [SerializeField] private bool saysLastLine;             // Check this if the speaker closes the convo
    [Header("Textbox Details")]
    [SerializeField] private float xOffset = 1f;    // Offset of text bubble from parent
    [SerializeField] private float yOffset = 1f;
    [SerializeField] public float waitToReadTime = 3f; // How long the textbox should remain onscreen after it's done reading
    [Header("Dialogue File")]
    [SerializeField] private TextAsset linesTXT;       // .txt file for lines

    [Header("Other Details")]
    [SerializeField] private bool playerControlled; // Does the text need player input to continue?
    [SerializeField] private bool conversation;     // Is this person in a conversation?
    [SerializeField] private GameObject partnerBubble;
    [SerializeField] private DialogueManager convoPartner;   // If so, they need someone to talk to
    [SerializeField] private AudioClip voice;

    private Transform parent;                       // Transform of the parent for positioning
    public string[] lines;                         // The actual lines in a usable form
    private int currentLine = 0;
    private bool activeCoroutine;
    private AudioSource voiceSource;

    public delegate void OnConvoOver();
    public static event OnConvoOver onConvoOver;

    private void Start()
    {
        this.transform.position = Vector3.zero;
        parent = GetComponentInParent<Transform>();
        if (isFacingRight)
            this.transform.localPosition = new Vector3(parent.position.x + xOffset, parent.position.y + yOffset, parent.position.z);
        else
        {
            this.transform.localPosition = new Vector3(parent.position.x - xOffset, parent.position.y + yOffset, parent.position.z);
            mainBox.flipX = true;
        }

        speakerNameText.text = speakerName;
        //speakerWords.enableAutoSizing = false;
        getFileContents();
        voiceSource = GetComponent<AudioSource>();
        activeCoroutine = false;
        this.gameObject.SetActive(false);

        /*if (startActivated)
        {
            StartCoroutine(DoAutoActivation());
        }*/
        //this.gameObject.SetActive(false);
        

        
        /*else
        {

        }*/
    }

    private void Update()
    {
        /*if (playerControlled)
        {
            if (activeCoroutine)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    speakerWords.text = lines[currentLine].Substring(1, lines[currentLine].Length - 1);
                    activeCoroutine = false;
                }
            }
        }*/
        
    }

    // Public methods --------------------------------------------------------
    public void ReadNextLine()  // Call this to read the next line of dialogue in the text doc
    {
        StopAllCoroutines();
        //speakerWords.enableAutoSizing = true;
        speakerWords.text = "";                 // Clear the bubble so it can be read out
        this.gameObject.SetActive(true);        // Show the dialogue bubble
        getFileContents();
        StartCoroutine(DoReadLine(lines[currentLine]));
        
    }

    // Private methods --------------------------------------------------------
    private void getFileContents()  // Splits up the text file into usable lines
    {
        lines = linesTXT.text.Split('\n');
    }

    // Coroutines --------------------------------------------------------------
    IEnumerator DoAutoActivation()  // Waits for the given amount of time, then starts the dialogue sequence
    {
        yield return new WaitForSeconds(waitToReadTime);
        ReadNextLine();
    }

    IEnumerator DoReadLine(string line)
    {
        activeCoroutine = true;
        var numCharsRevealed = 1;               // Read out the text letter by letter
        while (numCharsRevealed < line.Length)
        {
            voiceSource.PlayOneShot(voice);
            speakerWords.text = line.Substring(1, numCharsRevealed);
            ++numCharsRevealed;
            /*if (Input.GetButtonDown("Interact"))
            {
                speakerWords.text = lines[currentLine].Substring(1, lines[currentLine].Length - 1);
                break;
            }*/
            yield return new WaitForSeconds(0.05f);
        }
        //yield return new WaitForSeconds(0.25f);
        if (playerControlled)   // If the player needs to advance the text, instead of it happening naturally
        {
            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        }
        else
        {
            yield return new WaitForSeconds(waitToReadTime);     // Wait so the player can read it
        }

        currentLine++;      // Increment the line we're on so we read the next line next time
        
        if (currentLine >= lines.Length - 1)
        {  
            if (saysLastLine)
            {
                Debug.Log("HEWWO");
                this.gameObject.SetActive(false);
                StopAllCoroutines();
            }
            // Need to do something to end the convo
        }

        if (conversation) // Pass it over to the other person!
        {
            if (line.Contains('$')) // If the line starts with a $, it's a self pass
            {
                ReadNextLine();
            }
            else if (line.Contains('@'))
            {
                onConvoOver?.Invoke();
                this.gameObject.SetActive(false);
            }
            else // If it starts with a %, pass it to the convo partner
            {
                partnerBubble.SetActive(true);
                convoPartner.ReadNextLine();
                this.gameObject.SetActive(false);        // Hide the dialogue bubble
            }
            
        }
        
        activeCoroutine = false;

        yield return null;
    }
}

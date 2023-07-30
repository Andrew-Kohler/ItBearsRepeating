using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    // Place this on an empty gameobject that represents the centerpoint of an arena
    [Header("Player and Activation Details")]
    [SerializeField] GameObject player;             // The player (activator of the arena)
    [SerializeField] GameObject targetGroupObj;     // The camera target group (to shift the focus to)
    [SerializeField] float activationDistance = 20f;
    [SerializeField] int pointNumber;               // Which camera change this is, ordered from first to last in the level

    [Header("Arena Properties")]
    [SerializeField] GameObject leftBound;
    [SerializeField] GameObject rightBound;
    [SerializeField] private bool hasDoors;   // Does this arena have doors you need to bust out of?
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("Fight Properties")]
    [SerializeField] private string bossName;
    [SerializeField] private string bossSubheader;
    [SerializeField] private GameObject multimanBrawl; // Is this a one-on-one fight, or is it a horde battle
    [SerializeField] private Stack<GameObject> enemies; // All the enemies participating
    [SerializeField] private BossHealthBar healthBarController;    // The boss health bar
    private float totalMaxHealth;
    private float totalCurrentHealth;

    Cinemachine.CinemachineTargetGroup targetGroup;

    private bool camSwitch; // Have we moved the camera?
    private bool inCombat; // Are we in combat?

    void Start()
    {
        targetGroup = targetGroupObj.GetComponent<Cinemachine.CinemachineTargetGroup>();
        leftBound.SetActive(false);
        rightBound.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Activation is based on x only to avoid weird radius behavior
        if (Mathf.Abs(transform.position.x - player.transform.position.x) <= activationDistance && !camSwitch)
        {
            camSwitch = true;
            StopAllCoroutines();
            StartCoroutine(DoZoomToArena());
            leftBound.SetActive(true);
            rightBound.SetActive(true);
        }

        else if (inCombat) // This controls the spawning logic
        {

        }
        /*else if (Mathf.Abs(transform.position.x - player.transform.position.x) > activationDistance && camSwitch)
        {
            camSwitch = false;
            StopAllCoroutines();
            StartCoroutine(DoZoomIn());
        }*/
    }

    // Private methods
    private float calculateMaxHealth(Stack<GameObject> combatants) // Finds out the max health of what's happening in this battle
    {

        return 0;
    }

    // Coroutines ----------------------------------------------------------------------

    IEnumerator DoZoomToArena() // Switch from looking at the player to looking at the arena
    {
        //float lerpVal = 0f;
        while (targetGroup.m_Targets[pointNumber].weight < 1f)
        {
            targetGroup.m_Targets[pointNumber].weight = Mathf.Lerp(targetGroup.m_Targets[pointNumber].weight, 1f, .01f);
            targetGroup.m_Targets[0].weight = Mathf.Lerp(targetGroup.m_Targets[0].weight, 0f, .01f);
            yield return null;
        }

        yield return null;
    }

    IEnumerator DoZoomIn() // De-incorporate this point of interest from the camera view
    {
        //float lerpVal = 0f;
        while (targetGroup.m_Targets[pointNumber].weight > 0f)
        {
            targetGroup.m_Targets[pointNumber].weight = Mathf.Lerp(targetGroup.m_Targets[pointNumber].weight, 0f, .01f);
            yield return null;
        }

        yield return null;
    }
}

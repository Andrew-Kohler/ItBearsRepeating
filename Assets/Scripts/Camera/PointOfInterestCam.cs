using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestCam : MonoBehaviour
{
    // Place this on an empty gameobject that you want the camera to look at
    [SerializeField] GameObject player;
    [SerializeField] GameObject targetGroupObj;
    [SerializeField] float activationDistance = 20f;
    [SerializeField] int pointNumber;               // Which camera change this is, ordered from first to last in the level
    [SerializeField] bool cutscenePoint;

    [SerializeField] bool followPlayerX;            // Track the player's x position for a certain distance
    [SerializeField] float followDistance = 50f;    // How long the point follows the player for
    float lowerXBound;
    bool following;
    bool done;
    Cinemachine.CinemachineTargetGroup targetGroup;

    private bool camSwitch;

    void Start()
    {
        targetGroup = targetGroupObj.GetComponent<Cinemachine.CinemachineTargetGroup>();
        following = false;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cutscenePoint)
        {
            if (!followPlayerX) // If this is a static point
            {
                // Activation is based on x only to avoid weird radius behavior
                if (Mathf.Abs(transform.position.x - player.transform.position.x) <= activationDistance && !camSwitch)
                {
                    camSwitch = true;
                    StopAllCoroutines();
                    StartCoroutine(DoZoomOut());
                }
                else if (Mathf.Abs(transform.position.x - player.transform.position.x) > activationDistance && camSwitch)
                {
                    camSwitch = false;
                    StopAllCoroutines();
                    StartCoroutine(DoZoomIn());
                }
            }
            else // If this is not a static point
            {
                // Activation is based on x only to avoid weird radius behavior
                if (Mathf.Abs(transform.position.x - player.transform.position.x) <= activationDistance && !camSwitch && !done)
                {
                    camSwitch = true;
                    StopAllCoroutines();
                    lowerXBound = player.transform.position.x;
                    StartCoroutine(DoZoomOut());
                }
                if (following)
                {
                    transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                }

                if(player.transform.position.x < lowerXBound - 5f || player.transform.position.x > lowerXBound + followDistance)
                {
                    if (camSwitch)
                    {
                        camSwitch = false;
                        following = false;
                        done = true;
                        StopAllCoroutines();
                        StartCoroutine(DoZoomIn());
                    }
                }
                
            }
            
        }
        
    }

    public void CallZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(DoZoomOut());
    }

    public void CallZoomIn()
    {
        StopAllCoroutines();
        StartCoroutine(DoZoomIn());
    }

    IEnumerator DoZoomOut() // Incorporate this point of interest into the camera view
    {
        //float lerpVal = 0f;
        if (followPlayerX)
        {
            Debug.Log("Gamers?");
            following = true;
        }
        while (targetGroup.m_Targets[pointNumber].weight <= .98f) 
        {
            targetGroup.m_Targets[pointNumber].weight = Mathf.Lerp(targetGroup.m_Targets[pointNumber].weight, 1f, .01f);
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

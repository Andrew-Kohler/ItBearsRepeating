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
    Cinemachine.CinemachineTargetGroup targetGroup;

    private bool camSwitch;

    void Start()
    {
        targetGroup = targetGroupObj.GetComponent<Cinemachine.CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        // Activation is based on x only to avoid weird radius behavior
        if(Mathf.Abs(transform.position.x - player.transform.position.x) <= activationDistance && !camSwitch)
        {
            camSwitch = true;
            StopAllCoroutines();
            StartCoroutine(DoZoomOut());
        }
        else if(Mathf.Abs(transform.position.x - player.transform.position.x) > activationDistance && camSwitch)
        {
            camSwitch = false;
            StopAllCoroutines();
            StartCoroutine(DoZoomIn());
        }
    }

    IEnumerator DoZoomOut() // Incorporate this point of interest into the camera view
    {
        //float lerpVal = 0f;
        while(targetGroup.m_Targets[pointNumber].weight < 1f) 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAreaCamMovement : MonoBehaviour
{
    /*Cinemachine.CinemachineTargetGroup targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();

    Cinemachine.CinemachineTargetGroup.Target target;
    target.target = "Target to add";
    target.weight = "the weight";
    target.radius = "the radius";
     
            for (int i = 0; i<targetGroup.m_Targets.Length; i++)
            {
                if (targetGroup.m_Targets[i].target == null)
                {
                    targetGroup.m_Targets.SetValue(target, i);
                    return;
                }
            }

    targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
     
    Cinemachine.CinemachineTargetGroup.Target target;
    target.target = "Target to add";
    target.weight = "the weight";
    target.radius = "the radius";
     
            for (int i = 0; i<targetGroup.m_Targets.Length; i++)
            {
                if (targetGroup.m_Targets[i].target == null)
                {
                    targetGroup.m_Targets.SetValue(target, i);
                    return;
                }
            }*/
    // Ok, ok, so how's this gonna work
    // I think I add all the relevant targets to the group in the editor, just with everything but protag set to 0
    // Then, when I hit certain checkpoints, change where the camera is in a more significant way
    [SerializeField] GameObject player;
    [SerializeField] GameObject targetGroupObj;
    Cinemachine.CinemachineTargetGroup targetGroup;

    private bool camSwitch;

    void Start()
    {
        targetGroup = targetGroupObj.GetComponent<Cinemachine.CinemachineTargetGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) < 20f && !camSwitch)
        {
            camSwitch = true;
            StopAllCoroutines();
            StartCoroutine(DoZoomOut());
        }
        else if(Vector2.Distance(transform.position, player.transform.position) > 20f && camSwitch)
        {
            camSwitch = false;
            StopAllCoroutines();
            StartCoroutine(DoZoomIn());
        }
    }

    IEnumerator DoZoomOut()
    {
        //float lerpVal = 0f;
        while(targetGroup.m_Targets[1].weight < 1f) 
        {
            targetGroup.m_Targets[1].weight = Mathf.Lerp(targetGroup.m_Targets[1].weight, 1f, .01f);
            yield return null;
        }
        
        yield return null;
    }

    IEnumerator DoZoomIn()
    {
        //float lerpVal = 0f;
        while (targetGroup.m_Targets[1].weight > 0f)
        {
            targetGroup.m_Targets[1].weight = Mathf.Lerp(targetGroup.m_Targets[1].weight, 0f, .01f);
            yield return null;
        }

        yield return null;
    }
}

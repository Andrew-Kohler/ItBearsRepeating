using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamBoundBoxChanger : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineConfiner confiner;
    [SerializeField] CompositeCollider2D collider1;
    [SerializeField] CompositeCollider2D collider2;
    [SerializeField] GameObject collider2ActivePoint;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, collider2ActivePoint.transform.position) < 5f)
        {
            Debug.Log("Ah");
            confiner.m_BoundingShape2D = collider2;

        }
        
    }
}

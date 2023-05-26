using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackableAttack : MonoBehaviour
{
    [SerializeField] private float smackDMG = 1f;
    [SerializeField] private Vector3 smackKnockback = new Vector3(1f, 1f, 0f);
    public float DMG => smackDMG;
    public Vector3 Knockback => smackKnockback;
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}

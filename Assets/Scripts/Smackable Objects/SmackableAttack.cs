using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackableAttack : MonoBehaviour
{
    [SerializeField] private float smackDMG = 1f;
    [SerializeField] private Vector3 smackKnockback = new Vector3(1f, 1f, 0f);
    public float DMG => smackDMG;
    public Vector3 Knockback => smackKnockback;

    public void changeKnockbackDir(bool right)
    {
        if (!right)
            smackKnockback = new Vector3(Mathf.Abs(smackKnockback.x), Mathf.Abs(smackKnockback.y), 0f);
        else
            smackKnockback = new Vector3((-1f *smackKnockback.x), smackKnockback.y, 0f);
    }

    public void swapKnockbackDir()
    {
        if(smackKnockback.x > 0)
            smackKnockback = new Vector3((-1f * smackKnockback.x), smackKnockback.y, 0f);
        else
            smackKnockback = new Vector3(Mathf.Abs(smackKnockback.x), Mathf.Abs(smackKnockback.y), 0f);
    }

}

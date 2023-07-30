using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float springStrength;
    [SerializeField] private string springColor;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * springStrength, ForceMode2D.Impulse);
            StartCoroutine(DoBounceAnim()); 
        }
    }

    IEnumerator DoBounceAnim()
    {
        anim.Play(springColor + "Bounce");
        yield return new WaitForSeconds(.5f);
        anim.Play(springColor + "Idle");
        yield return null;
    }
}

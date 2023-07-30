using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TutorialAsset : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private SpriteRenderer sr;

    [SerializeField] private string assignedAnimation;
    [SerializeField] private float distanceToReveal = 10f;
    [SerializeField] private float fadeStep = .1f;
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshPro tmp;
    private bool visible;
    private bool activeCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        anim.Play(assignedAnimation);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);

        if(tmp != null)
        {
            tmp.faceColor = sr.color;
        }

        visible = false;
        activeCoroutine = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameplay() && !activeCoroutine)
        {
            if (Mathf.Abs(transform.position.x - player.position.x) < distanceToReveal && !visible) // If conditions are met to show
            {
                StartCoroutine(DoFadeIn());
            }
            else if (Mathf.Abs(transform.position.x - player.position.x) >= distanceToReveal && visible) // Else if we should no longer be showing
            {
                StartCoroutine(DoFadeOut());
            }
        }
        
    }

    IEnumerator DoFadeIn()
    {
        visible = true;
        activeCoroutine = true;
        while(sr.color.a < 1)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + (fadeStep * Time.deltaTime));
            tmp.faceColor = sr.color;
            yield return null;
        }
        activeCoroutine = false;
        yield return null;
    }

    IEnumerator DoFadeOut()
    {
        visible = false;
        activeCoroutine = true;
        while (sr.color.a > 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (fadeStep * Time.deltaTime));
            if(tmp != null)
                tmp.faceColor = sr.color;
            yield return null;
        }
        activeCoroutine = false;
        yield return null;
    }
        
}

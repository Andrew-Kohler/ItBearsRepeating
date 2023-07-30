using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicSpark : MonoBehaviour
{
    [SerializeField] private float period;
    private float countdown;
    private ParticleSystem sparks;
    private AudioSource audioS;

    [SerializeField] private AudioClip sparkSound1;
    [SerializeField] private AudioClip sparkSound2;
    void Start()
    {
        sparks = GetComponent<ParticleSystem>();
        audioS = GetComponent<AudioSource>();
        countdown = period;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0){
            sparks.Play();
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                audioS.PlayOneShot(sparkSound1);
            }
            else
            {
                audioS.PlayOneShot(sparkSound2);
            }
            countdown = period;
        }
    }
}

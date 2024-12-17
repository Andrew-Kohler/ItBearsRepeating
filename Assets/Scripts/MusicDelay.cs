using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDelay : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DoDelay()); 
        
    }

    IEnumerator DoDelay()
    {
        yield return new WaitForSeconds(10f);
        audioSource.Play();
        
    }
}

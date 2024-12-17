using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSwap : MonoBehaviour
{
    [SerializeField] AudioSource soundtrackDJ;
    [SerializeField] AudioClip newTrack;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            soundtrackDJ.clip = newTrack;
            soundtrackDJ.Play();
            this.gameObject.SetActive(false);   
        }
    }
}

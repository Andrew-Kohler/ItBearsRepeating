using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStatWheel : MonoBehaviour
{
    [SerializeField] Transform onscreen;
    [SerializeField] Transform offscreen;
    [SerializeField] float step = .05f;

    private void Start()
    {
        transform.position = offscreen.position;
    }
    public void MoveOnscreen()
    {
        StopAllCoroutines();
        StartCoroutine(DoMoveOnscreen());
    }

    public void MoveOffscreen()
    {
        StopAllCoroutines();
        StartCoroutine(DoMoveOffscreen());
    }

    IEnumerator DoMoveOnscreen()
    {
        Vector3 velocity = Vector3.zero;    // Initial velocity values for the damping functions

        while (Vector3.Distance(transform.position, onscreen.position) >= .05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, onscreen.position, ref velocity, step); // Move camera position
            yield return null;
        }

        yield return null;
    }

    IEnumerator DoMoveOffscreen()
    {
        Vector3 velocity = Vector3.zero;    // Initial velocity values for the damping functions

        while (Vector3.Distance(transform.position, offscreen.position) >= .05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, offscreen.position, ref velocity, step); // Move camera position
            yield return null;
        }

        yield return null;
    }
}

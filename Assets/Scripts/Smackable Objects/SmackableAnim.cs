using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackableAnim : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movingRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed)); 
/*        if(rotationSpeed > 0)                   // A very bandaid way of slowing the roll for now
            rotationSpeed -= Time.deltaTime;*/
    }

    public void changeRotationSpeed(int choice) // 0 = stop, 1 = left, 2 = right;
    {
        if (choice == 0)
        {
            rotationSpeed = 0;
        }
        else if (choice == 1)
        {
            rotationSpeed = -movingRotationSpeed;
        }
        else if (choice == 2)
        {
            rotationSpeed = movingRotationSpeed;
        }
    }

    public void swapRotationDirection()
    {
        rotationSpeed = -rotationSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SkySunset : MonoBehaviour
{
    [SerializeField] SpriteRenderer bottom;
    [SerializeField] SpriteRenderer top;
    private float timeToSet = 180f; // 3.5 min
    private float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // Start the day at noon
        bottom.color = new Color(bottom.color.r, bottom.color.g, bottom.color.b, 100);
        top.color = new Color(bottom.color.r, bottom.color.g, bottom.color.b, 100);
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGameplay() && bottom.color.a >= .15f)
        {
            var someValueFrom0To1 = currentTime / timeToSet;
            var newAlpha = math.remap(0f, 1f, 1f, .15f, someValueFrom0To1);

            bottom.color = new Color(bottom.color.r, bottom.color.g, bottom.color.b, newAlpha);
            top.color = new Color(bottom.color.r, bottom.color.g, bottom.color.b, newAlpha);

            currentTime += Time.deltaTime;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=rq6yGh-piIU
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = .25f;

    private SpriteRenderer sr;
    private Material material;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();    
        material = sr.material;
    }

    // Public methods

    public void CallDamageFlash()
    {
        StartCoroutine(DoDamageFlash());
    }

    // Private methods -----------------------------

    private void SetFlashColor()    // Set the color of the flash
    {
        material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFlashAmount(float amount) // Set the amount the flash is there
    {
        material.SetFloat("_FlashAmount", amount);
    }

    // Coroutines -------------------------------

    private IEnumerator DoDamageFlash()
    {
        // Set the color
        SetFlashColor();

        // LERP the flash amt
        float currentFlashAmt = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < _flashTime)
        {
            // Iterate elapsed
            elapsedTime += Time.deltaTime;

            //Lerp flash amt
            currentFlashAmt = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmt);

            yield return null;
        }
    }

}

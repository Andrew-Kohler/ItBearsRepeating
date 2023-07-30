using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float explosionDMG = 10f;
    [SerializeField] public Vector3 explosionKnockback = new Vector3(1f, 1f, 0f);

	private Animator anim;

    private void Start()
    {
		anim = GetComponent<Animator>();
		StartCoroutine(DoDuration());
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<PlayerTakeDamage>().TakeDamage(this.gameObject);
			//StartCoroutine(DoWaitForShake());
		}

	}

	public Explosion(float damageValue, Vector3 knockback, bool knockbackDirRight)
	{
		explosionDMG = damageValue;
		explosionKnockback = knockback;
        if (!knockbackDirRight)
        {
			explosionKnockback = explosionKnockback * -1;

		}
	}

	public void SetValues(float damageValue, Vector3 knockback, bool knockbackDirRight)
	{
		explosionDMG = damageValue;
		explosionKnockback = knockback;
		if (!knockbackDirRight)
		{
			explosionKnockback = explosionKnockback * -1;

		}

	}

	IEnumerator DoDuration()
    {
		anim.Play("Explosion");
		yield return new WaitForSeconds(.3f);
		GetComponent<SpriteRenderer>().enabled = false;	
		GetComponent<CircleCollider2D>().enabled = false;
		yield return new WaitForSeconds(.4f);
		Destroy(gameObject);
    }
}

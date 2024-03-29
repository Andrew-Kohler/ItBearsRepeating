using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float damageValue;
	public float duration;
	public Vector3 knockback;

	private Rigidbody2D rb;
	private float initialRot;
	private bool right;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		StartCoroutine(Duration());
	}

    private void Update()
    {
		
		if (damageValue == 10)  // Easy rocket check
		{
			if (rb.velocity.y <= 0) //If the rocket is falling
			{
				
				if (right) // If facing right
                {
					if (transform.rotation.eulerAngles.z < 45 || transform.rotation.eulerAngles.z > 290)
						transform.Rotate(Vector3.forward * (rb.velocity.y) / 16);
				}
                else 
                {
					if (transform.rotation.eulerAngles.z < 250)
						transform.Rotate(Vector3.forward * (-rb.velocity.y) / 16);
				}
			}
			else if (rb.velocity.y > 0) // If the rocket is rising
			{
				if (right) // If facing right
				{
					if (transform.rotation.eulerAngles.z >= 0)
						transform.Rotate(Vector3.forward * (-rb.velocity.y) / 40);
				}
				else
				{
					if (transform.rotation.eulerAngles.z <= 180)
						transform.Rotate(Vector3.forward * (rb.velocity.y) / 40);
				}
							

			}
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			collision.gameObject.GetComponent<PlayerTakeDamage>().TakeDamage(this.gameObject);
			StartCoroutine(DoWaitForShake());
		}
        if (collision.gameObject.CompareTag("Terrain") && damageValue == 10)
        {
			GameObject explosion = (GameObject)Instantiate(Resources.Load("Explosion"), this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
			explosion.GetComponent<Explosion>().SetValues(10f, new Vector3(10f, 10f, 0f), right);
			Destroy(gameObject);
		}
		
	}

    public Projectile(float duration, float damageValue, Vector3 knockback)
	{
		this.duration = duration;
		this.damageValue = damageValue;
		this.knockback = knockback;
	}

	public void SetValues(float duration, float damageValue, Vector3 knockback, bool right)
	{
		this.duration = duration;
		this.damageValue = damageValue;
		this.knockback = knockback;
		this.right = right;
	}

	IEnumerator Duration()
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}

	IEnumerator DoWaitForShake()
    {
		yield return new WaitForSeconds(.1f);
		Destroy(gameObject);
	}

}

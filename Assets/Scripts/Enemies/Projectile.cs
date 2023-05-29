using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float damageValue;
	public float duration;
	public Vector3 knockback;

	private void Start()
	{
		StartCoroutine(Duration());
	}

	public Projectile(float duration, float damageValue, Vector3 knockback)
	{
		this.duration = duration;
		this.damageValue = damageValue;
		this.knockback = knockback;
	}

	public void SetValues(float duration, float damageValue, Vector3 knockback)
	{
		this.duration = duration;
		this.damageValue = damageValue;
		this.knockback = knockback;
	}

	IEnumerator Duration()
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}

}

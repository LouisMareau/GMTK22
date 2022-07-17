using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankyEnemy : Enemy
{
	[Header("DICE")]
	[SerializeField] private GameObject _dicePrefab;

	[Header("VFXs")]
	[SerializeField] private GameObject _explosionPrefab;

	public override void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Instantiate<GameObject>(_dicePrefab, _rootTransform.position, _rootTransform.rotation);
			Kill();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.data.LoseLife(3);
			Instantiate<GameObject>(_explosionPrefab, collision.transform.position, _explosionPrefab.transform.rotation);
			Destroy(gameObject);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : Enemy
{
	[Header("VFXs")]
	[SerializeField] private GameObject _explosionPrefab;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.playerData.LoseLife();
			Instantiate<GameObject>(_explosionPrefab, collision.transform.position, _explosionPrefab.transform.rotation);
			Destroy(gameObject);
		}
	}
}
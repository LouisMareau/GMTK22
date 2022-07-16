using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : Enemy
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			PlayerData player = collision.gameObject.GetComponent<PlayerData>();
			player.LoseLife();
			// VFX EXPLOSION
			Destroy(gameObject);
		}
	}
}
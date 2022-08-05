using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeDetonator : Enemy
{
	#region GAMEPLAY
	protected override void Kill(float delay = 0)
	{
		GameRecords.enemyMeleeDetonatorKilled++;

		base.Kill(delay);
	}
	#endregion

	#region COLLISION EVENTS
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();
			player.data.LoseLife(damage);
			player.PlayAnim_HitSparks();

			Kill();
		}
	}
	#endregion
}
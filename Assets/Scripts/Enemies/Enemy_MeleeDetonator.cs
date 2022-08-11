using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeDetonator : Enemy
{
	#region INITIALIZATION
	protected override void Awake()
	{
		base.Awake();

		BaseDataManager.BaseData_Enemy_MeleeDetonator baseData = BaseDataManager.baseDataEnemy_MeleeDetonator;
		health = baseData.health;
		speed = baseData.speed;
		damage = baseData.damage;
		scoreWhenKilled = baseData.scoreWhenKilled;
		rotationalSpeed = baseData.rotationalSpeed;
	}
	#endregion

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
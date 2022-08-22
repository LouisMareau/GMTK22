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
	public override void UpdateData(float health, float speed, int damage, int scoreWhenKilled, float rotationalSpeed)
	{
		base.UpdateData(health, speed, damage, scoreWhenKilled, rotationalSpeed);

		HUDManager.overlayData.enemyMDHealth.Set("Health", health);
		HUDManager.overlayData.enemyMDSpeed.Set("Speed", speed);
		HUDManager.overlayData.enemyMDDamage.Set("Damage", damage);
		HUDManager.overlayData.enemyMDScoreOnKill.Set("Score On Kill", scoreWhenKilled);
		HUDManager.overlayData.enemyMDRotationSpeed.Set("Rotation Speed", rotationalSpeed);
	}

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
			player.data.LoseHealth(damage);
			player.PlayAnim_HitSparks();

			Kill();
		}
	}
	#endregion
}
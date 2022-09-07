using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DieHolder : Enemy
{
	[Header("DICE")]
	[SerializeField] private GameObject _associatedDice;

	#region INITIALIZATION
	protected override void Awake()
	{
		base.Awake();

		BaseDataManager.BaseData_Enemy_DieHolder baseData = BaseDataManager.baseDataEnemy_DieHolder;
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

		HUDManager.overlayData.enemyDHHealth.Set("Health", health);
		HUDManager.overlayData.enemyDHSpeed.Set("Speed", speed);
		HUDManager.overlayData.enemyDHDamage.Set("Damage", damage);
		HUDManager.overlayData.enemyDHScoreOnKill.Set("Score On Kill", scoreWhenKilled);
		HUDManager.overlayData.enemyDHRotationSpeed.Set("Rotation Speed", rotationalSpeed);
	}

	public override void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			LaunchDice();
			Kill();
		}
	}

	private void LaunchDice()
	{
		// We remove the dice object from the enemy hierarchy
		_associatedDice.transform.SetParent(StaticReferences.Instance.diceContainer);

		Dice die = _associatedDice.GetComponent<Dice>();

		// We setup and apply physics forces onto the die
		die.rigidbody.isKinematic = false;
		die.rigidbody.AddForce(Vector3.up * Random.Range(800f, 1200f), ForceMode.Impulse);
		Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(80000f, 300000f);
		die.rigidbody.AddTorque(randomTorque);

		// We set a delay before destroying the die (if not picked up by the player)
		die.KillAfterDelay(12.0f);
	}

	protected override void Kill(float delay = 0)
	{
		GameRecords.enemyDieHolderKilled++;

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

			LaunchDice();
			Kill();
		}
	}
	#endregion
}

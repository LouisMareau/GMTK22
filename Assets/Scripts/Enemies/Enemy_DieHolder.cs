using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DieHolder : Enemy
{
	[Header("DICE")]
	[SerializeField] private GameObject _associatedDice;

	#region GAMEPLAY
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

		Die6 die = _associatedDice.GetComponent<Die6>();

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
			player.data.LoseLife(damage);

			LaunchDice();
			Kill();
		}
	}
	#endregion
}

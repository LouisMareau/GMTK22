using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankyEnemy : Enemy
{
	[Header("DICE")]
	[SerializeField] private GameObject _dice;

	public override void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Explode();
			LaunchDice();
			Kill();
		}
	}

	private void LaunchDice()
	{
		// We remove the dice object from the enemy hierarchy
		_dice.transform.SetParent(StaticReferences.Instance.diceContainer);
		Die die = _dice.GetComponent<Die>();
		die.rigidbody.isKinematic = false;
		die.rigidbody.AddForce(Vector3.up * 1000f, ForceMode.Impulse);
		Vector3 randomTorque = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(8000f, 30000f);
		die.rigidbody.AddTorque(randomTorque);
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();
			player.data.LoseLife(3);

			Explode();
			LaunchDice();
			Kill();
		}
	}
}
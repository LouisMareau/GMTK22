using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ENUM
public enum EnemyType
{
	SMALL,
	TANK
}
#endregion

public abstract class Enemy : MonoBehaviour
{
	[Header("HEALTH")]
	public float health = 1;

	[Header("MOVEMENT")]
	public float speed = 5f;
	[Space]
	public EnemyType type;
	private Transform _player;

	private void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		this.transform.Translate((_player.position - this.transform.position).normalized * speed * Time.deltaTime);
	}

	public void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
			Kill();
	}

	public void Kill() { Destroy(gameObject); }
}
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

	protected Transform _rootTransform;
	protected Transform _meshTransform;
	protected Transform _playerTransform;

	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.GetChild(0); }
		if (_playerTransform == null) { _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; }
	}

	private void Update()
	{
		this.transform.Translate((_playerTransform.position - this.transform.position).normalized * speed * Time.deltaTime);
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
			Kill();
	}

	public void Kill() { Destroy(gameObject); }
}
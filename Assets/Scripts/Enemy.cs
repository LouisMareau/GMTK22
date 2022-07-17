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
	public EnemyType type;

	[Header("HEALTH")]
	public float baseHealth = 1;
	public float health;

	[Header("MOVEMENT")]
	public float baseSpeed = 5f;
	public float speed;

	[Header("VFXs")]
	[SerializeField] private GameObject _explosionPrefab;

	[Header("SCORING")]
	public int scoreWhenKilled = 1;

	protected Transform _rootTransform;
	protected Transform _meshTransform;
	protected Transform _playerTransform;
	protected Collider _collider;

	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.GetChild(0); }
		if (_playerTransform == null) { _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; }
		if (_collider == null) { _collider = GetComponent<SphereCollider>(); }

		health = baseHealth;
		speed = baseSpeed;
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

	protected void Explode()
	{
		_collider.enabled = false;

		// VFX: Explosion
		Instantiate(_explosionPrefab, _meshTransform.position, _explosionPrefab.transform.rotation, StaticReferences.Instance.vfxContainer);
	}

	protected void Kill()
	{
		int s = GameManager.Instance.score += scoreWhenKilled;
		HUDManager.Instance.UpdateScoreLabel(s);

		Destroy(gameObject);
	}

	protected abstract void OnTriggerEnter(Collider other);

	public virtual void IncreaseDifficulty(float multiplier)
	{
		speed *= multiplier;
	}
}
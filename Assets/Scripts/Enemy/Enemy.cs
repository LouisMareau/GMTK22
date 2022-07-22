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

	[Header("DATA")]
	[SerializeField] protected float _baseHealth = 1f;
	[SerializeField] protected float _baseSpeed = 5f;
	[SerializeField] protected int _baseDamage = 1;
	public float health { get; protected set; }
	public float speed { get; protected set; }
	public int damage { get; protected set; }
	public Vector3 currentDirection { get; protected set; }
	public float distanceFromPlayer { get; protected set; }

	[Header("VFXs")]
	[SerializeField] protected GameObject _prefabVFX_spawn;
	[SerializeField] protected GameObject _prefabVFX_death;

	[Header("SCORING")]
	[SerializeField] protected int _baseScoreWhenKilled = 1;
	public int scoreWhenKilled { get; protected set; }

	[Header("LOCAL REFERENCES")]
	[SerializeField] protected Transform _rootTransform;
	[SerializeField] protected Transform _meshTransform;
	[SerializeField] protected Collider _collider;

	protected Transform _playerTransform;

	#region INITILIZATION
	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.GetChild(0); }
		if (_collider == null) { _collider = GetComponent<SphereCollider>(); }
		if (_playerTransform == null) { _playerTransform = StaticReferences.Instance.playerTransform; }

		health = _baseHealth;
		speed = _baseSpeed;
		damage = _baseDamage;
		scoreWhenKilled = _baseScoreWhenKilled;
	}
	#endregion

	protected virtual void Update()
	{
		if (_playerTransform == null) return;

		currentDirection = CalculateDirection(_playerTransform.position);
		distanceFromPlayer = CalculateDistanceFromPlayer();

		MoveTowards(currentDirection, speed);
	}

	#region GAMEPLAY
	protected Vector3 CalculateDirection(Vector3 target)
	{
		return (target - _rootTransform.position).normalized;
	}
	protected float CalculateDistanceFromPlayer()
	{
		return (_playerTransform.position - _rootTransform.position).magnitude;
	}

	protected virtual void MoveTowards(Vector3 direction, float speed)
	{
		_rootTransform.Translate(direction * speed * Time.deltaTime);
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
			Kill();
	}

	protected virtual void Kill(float delay = 0f)
	{
		// Score
		int s = GameManager.Instance.score += scoreWhenKilled;
		HUDManager.Instance.UpdateScoreLabel(s);

		// Update game records
		GameRecords.enemiesKilledSinceLastFrame++;

		PlayAnim_Death();

        EnemySpawner.Instance.removeEnemy(this);
		Destroy(gameObject, delay);
	}
	#endregion

	#region COLLISION EVENTS
	protected abstract void OnTriggerEnter(Collider other);
	#endregion

	#region ANIMATIONS / VFX
	protected virtual void PlayAnim_Spawn()
	{
		// VFX: Spawn
	}

	protected virtual void PlayAnim_Death()
	{
		_collider.enabled = false;

		// VFX: Death
		Instantiate(_prefabVFX_death, _meshTransform.position, _prefabVFX_death.transform.rotation, StaticReferences.Instance.vfxContainer);
	}
	#endregion
}

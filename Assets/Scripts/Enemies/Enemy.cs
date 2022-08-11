using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ENUM
public enum EnemyType
{
	DIE_HOLDER,
	MELEE_DETONATOR,
	PULSAR
}
#endregion

public abstract class Enemy : MonoBehaviour
{
	public EnemyType type;

	public float health { get; protected set; }
	public float speed { get; protected set; }
	public int damage { get; protected set; }
	public int scoreWhenKilled { get; protected set; }
	public float rotationalSpeed { get; protected set; }

	public Vector3 currentDirection { get; protected set; }
	public float distanceFromPlayer { get; protected set; }

	[Header("VFXs")]
	[SerializeField] protected GameObject _prefabVFX_spawn;
	[SerializeField] protected GameObject _prefabVFX_death;

	[Header("SCORING")]
	[SerializeField] protected int _baseScoreWhenKilled = 1;

	[Header("LOCAL REFERENCES")]
	[HideInInspector] public Transform targetTransform;
	[HideInInspector] public Transform _rootTransform;
	[SerializeField] protected Transform _meshTransform;
	[SerializeField] protected Collider _collider;

	protected Transform _playerTransform;

	#region INITILIZATION
	protected virtual void Awake()
	{
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.GetChild(0); }
		if (_collider == null) { _collider = GetComponent<SphereCollider>(); }
		if (_playerTransform == null) { _playerTransform = StaticReferences.Instance.playerTransform; }
		if (targetTransform == null) { targetTransform = _meshTransform; }
	}
	#endregion

	protected virtual void Update()
	{
		if (_playerTransform == null) return;

		if (GameManager.IsPlaying)
		{
			currentDirection = CalculateDirection(_playerTransform.position);
			distanceFromPlayer = CalculateDistanceFromPlayer();

			MoveTowards(currentDirection, speed);
			RotateTowards(_playerTransform.position, rotationalSpeed);
		}
	}

	public virtual void UpdateData(float health, float speed, int damage, int scoreWhenKilled, float rotationalSpeed)
	{
		this.health = health;
		this.speed = speed;
		this.damage = damage;
		this.scoreWhenKilled = scoreWhenKilled;
		this.rotationalSpeed = rotationalSpeed;
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

	protected virtual void RotateTowards(Vector3 target, float angularSpeed)
	{
		Vector3 targetDirection = target - _rootTransform.position;
		Vector3 lookDirection = Vector3.RotateTowards(_meshTransform.forward, targetDirection, angularSpeed * Time.deltaTime, 0.0f);

		_meshTransform.rotation = Quaternion.LookRotation(lookDirection);
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
		int s = GameRecords.score += scoreWhenKilled;
		HUDManager.Instance.UpdateScoreLabel(s);

		// Game records
		GameRecords.enemiesKilledSinceLastFrame++;

		PlayAnim_Death();

        EnemySpawner.Instance.RemoveEnemy(this);
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

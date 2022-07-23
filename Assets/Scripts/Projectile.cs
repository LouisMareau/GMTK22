using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("CORE")]
	public float speed = 10f;
	public float knockback = 0;

	private float _damage;
	private Vector3 _direction;
	[SerializeField] private float _timeBeforeDestruction = 3f;

	[Header("SEEKING")]
	private bool _isSeeker = false;
	[SerializeField] private float _angularVelocity = 1f;

	[SerializeField] private Transform _rootTransform;
	[SerializeField] private Transform _meshTransform;

	private Vector3 previousPosition;

	#region INITIALIZATION
	private void Awake()
	{
		if (_rootTransform == null) _rootTransform = transform;
		if (_meshTransform == null) _meshTransform = transform.GetChild(0);
	}

	private void Start()
	{
		previousPosition = _rootTransform.position;
	}

	public void Initialize(Vector3 origin, Vector3 direction, float damage)
	{
		_damage = damage;
		_direction = direction.normalized;
		_rootTransform.position = origin;

		Kill(_timeBeforeDestruction);
	}
	#endregion

	public void SetSeeker(bool shouldSeek)
	{
		_isSeeker = shouldSeek;
	}

	private void Update()
	{
		previousPosition = _rootTransform.position;

		if (_isSeeker)
		{
			// We find the closest enemy and update the direction
			Enemy target = EnemySpawner.Instance.FindClosestEnemy(_rootTransform.position);

			if (target != null)
				_direction = Vector3.RotateTowards(_direction, target.transform.position - _rootTransform.position, _angularVelocity, 1).normalized;
		}

		// We calculate and translate to the new position
		_rootTransform.Translate(_direction * speed * Time.deltaTime);

		// We need to raycast from the previous position to the current position of the projectile
		Ray ray = new Ray(previousPosition, (_rootTransform.position - previousPosition).normalized);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, (previousPosition - _rootTransform.position).magnitude))
		{
			if (hit.collider.tag == "Enemy")
			{
				var enemy = hit.collider.GetComponent<Enemy>();
				enemy.TakeDamage(_damage);
				// Knockback
				enemy.transform.Translate(-enemy.currentDirection * knockback);

				Kill();
			}
		}
	}

	public void Kill(float delay = 0)
	{
		Destroy(gameObject, delay);
	}

	//HACKY solution because if the projectile is on auto the raycast will be from inside
	//the collider and it can't detect the enemy
	//!!! not being triggered
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy") {
			this.SetSeeker(false);
			this._direction.z = 1;
		}
	}
}

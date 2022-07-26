using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	[Header("CORE")]
	[SerializeField] protected float _speed = 10f;

	[SerializeField] protected float _timeBeforeDestruction = 3f;
	protected Vector3 _direction;
	protected float _damage;

	[Header("VFX")]
	[SerializeField] protected GameObject _prefabVFX_death;

	[Header("LOCAL REFERENCES")]
	[SerializeField] protected Transform _rootTransform;
	[SerializeField] protected Transform _meshTransform;

	private Vector3 previousPosition;

	#region INITIALIZATION
	private void Awake()
	{
		if (_rootTransform == null) _rootTransform = transform;
		if (_meshTransform == null) _meshTransform = transform.GetChild(0);
	}

	protected virtual void Start()
	{
		previousPosition = _rootTransform.position;
	}

	public virtual void Initialize(Vector3 origin, Vector3 direction, float damage)
	{
		_damage = damage;
		_direction = direction.normalized;
		_rootTransform.position = origin;

		KillAfterDelay(_timeBeforeDestruction);
	}
	#endregion

	protected virtual void Update()
	{
		previousPosition = _rootTransform.position;

		// We calculate and translate to the new position
		_rootTransform.Translate(_direction * _speed * Time.deltaTime);

		// We need to raycast from the previous position to the current position of the projectile
		Ray ray = new Ray(previousPosition, (_rootTransform.position - previousPosition).normalized);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, (_rootTransform.position - previousPosition).magnitude))
		{
			if (hit.collider.tag == "Enemy")
			{
				Enemy enemy = hit.collider.GetComponent<Enemy>();

				enemy.TakeDamage(_damage);

				Kill();
			}
		}
	}

	public void Kill()
	{
		Destroy(gameObject);
	}
	private void KillAfterDelay(float delay = 0) { StartCoroutine(KillAfterDelay_Coroutine(delay)); }
	private IEnumerator KillAfterDelay_Coroutine(float delay)
	{
		float timer = 0;
		while (timer < delay)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		// VFX: Death
		Instantiate<GameObject>(_prefabVFX_death, _rootTransform.position, _rootTransform.rotation, StaticReferences.Instance.vfxContainer);

		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_rootTransform.position, (previousPosition - _rootTransform.position));
	}
}

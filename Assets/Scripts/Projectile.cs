using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("DATA")]
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _timeBeforeDestruction = 3f;

	[Header("VFXs")]
	[SerializeField] private GameObject _bloodSplatterPrefab;

	[SerializeField] private Transform _rootTransform;
	[SerializeField] private Transform _meshTransform;
	private Vector3 _direction;
	private float _damage;

	private void Awake()
	{
		if (_rootTransform == null) _rootTransform = transform;
		if (_meshTransform == null) _meshTransform = transform.GetChild(0);
	}

	public void Initialize(Vector3 origin, Vector3 direction, float damage)
	{
		_rootTransform.position = origin;
		_direction = direction.normalized;
		_damage = damage;

		Destroy(gameObject, _timeBeforeDestruction);
	}

	private void Update()
	{
		_rootTransform.Translate(_direction * _speed * Time.deltaTime);

		Ray ray = new Ray(_meshTransform.position, -_direction);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 2f))
		{
			if (hit.collider.tag == "Enemy")
			{
				hit.collider.GetComponent<Enemy>().TakeDamage(_damage);
				Instantiate<GameObject>(_bloodSplatterPrefab, hit.point, Quaternion.LookRotation(_direction));
				Destroy(gameObject);
			}
		}
	}
}
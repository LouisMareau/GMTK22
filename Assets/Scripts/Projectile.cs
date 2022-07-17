using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[Header("DATA")]
	public float speed = 10f;
	[SerializeField] private float _timeBeforeDestruction = 3f;

	[Header("VFXs")]
	[SerializeField] private GameObject _bloodSplatterPrefab;
	[SerializeField] private GameObject _sparksPrefab;

	[SerializeField] private Transform _rootTransform;
	[SerializeField] private Transform _meshTransform;
	private Vector3 _direction;
	private float _damage;
    public float knockback = 0;

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
		_rootTransform.Translate(_direction * speed * Time.deltaTime);

		Ray ray = new Ray(_meshTransform.position, -_direction);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, speed / 10))
		{
			if (hit.collider.tag == "Enemy")
			{
				Vector3 targetLocation = new Vector3(hit.point.x, 1, hit.point.z);

				var enemy = hit.collider.GetComponent<Enemy>();
                enemy.TakeDamage(_damage);
				// Knockback
				enemy.transform.Translate(-enemy.currentDirection * knockback);
                
				if (enemy.health <= 0)
					Instantiate<GameObject>(_bloodSplatterPrefab, targetLocation, Quaternion.LookRotation(_direction), StaticReferences.Instance.vfxContainer);
				else
					Instantiate<GameObject>(_sparksPrefab, targetLocation, Quaternion.LookRotation(_direction), StaticReferences.Instance.vfxContainer);

				Destroy(gameObject);
			}
		}
	}
}

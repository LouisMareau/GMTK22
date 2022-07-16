using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float timeBeforeDestruction = 3f;

	private Transform _transform;
	private Vector3 _direction;

	private void Awake()
	{
		_transform = transform;
	}

	public void Initialize(Vector3 origin, Vector3 direction)
	{
		_transform.position = origin;
		_direction = direction;

		Destroy(gameObject, timeBeforeDestruction);
	}

	private void Update()
	{
		_transform.Translate(_direction * speed * Time.deltaTime);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("MOVEMENT")]
	public float speed;

	[Header("SHOOTING")]
	[SerializeField] private GameObject projectilePrefab;

    private Transform _transform;
	private Transform _camera;

	private RaycastHit _hit;

	private void Awake()
	{
		_transform = this.transform;
		_camera = Camera.main.transform;
	}

    // Update is called once per frame
    void Update()
    {
		Move();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out _hit))
		{
			_transform.rotation = CalculateRotationAngle(_hit.point);

			Shoot(CalculateHitDirectionFromPlayer(_hit.point));
		}
    }

	private void Move()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		Vector3 moveDirection = Vector3.zero;

		if (x != 0)
			moveDirection.x = x;

		if (y != 0)
			moveDirection.z = y; // We set the Z-axis because we are top-down

		_transform.Translate(moveDirection * speed * Time.deltaTime);
	}

	private void Shoot(Vector3 direction)
	{
		if (Input.GetAxis("Fire1") > 0)
		{
			Projectile instance = Instantiate<GameObject>(projectilePrefab).GetComponent<Projectile>();
			instance.Initialize(_transform.position, direction);
		}
	}

	private Quaternion CalculateRotationAngle(Vector3 hitPoint)
	{
		return Quaternion.LookRotation(CalculateHitDirectionFromPlayer(hitPoint));
	}

	private Vector3 CalculateHitDirectionFromPlayer(Vector3 hitPoint) { return (hitPoint - _transform.position).normalized; }
}

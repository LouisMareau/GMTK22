using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("MOVEMENT")]
	[SerializeField] private float _speed = 12f;

	[Header("SHOOTING")]
	[SerializeField] private GameObject _projectilePrefab;
	[Space]
	[SerializeField] private int _rateOfFire = 3;
	private int _lastRateOfFire;
	private float _nextTimeToFire;

	[Header("LOCAL REFERENCES")]
	[SerializeField] private Transform _mesh;

    private Transform _transform;
	private Transform _camera;

	private void Awake()
	{
		if (_transform == null) { _transform = this.transform; }
		if (_mesh == null) { _mesh = transform.GetChild(0); }
		if (_camera == null) { _camera = Camera.main.transform; }

		_lastRateOfFire = _rateOfFire;
	}

    // Update is called once per frame
    void Update()
    {
		if (_rateOfFire != _lastRateOfFire)
			HUDManager.Instance.SetRateOfFireLabel(_rateOfFire);

		Move();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			_mesh.rotation = CalculateRotationAngle(hit.point);
			Shoot(CalculateHitDirectionFromPlayer(hit.point));
		}

		_nextTimeToFire -= Time.deltaTime;
		if (_nextTimeToFire < 0)
			_nextTimeToFire = 0;
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

		_transform.Translate(moveDirection * _speed * Time.deltaTime);
	}

	private void Shoot(Vector3 direction)
	{
		if ((Input.GetAxis("Fire1") > 0) && (_nextTimeToFire <= 0))
		{
			Projectile instance = Instantiate<GameObject>(_projectilePrefab).GetComponent<Projectile>();
			instance.Initialize(_transform.position, direction);

			_nextTimeToFire = 1 / _rateOfFire;
		}
	}

	private Quaternion CalculateRotationAngle(Vector3 hitPoint)
	{
		return Quaternion.LookRotation(CalculateHitDirectionFromPlayer(hitPoint));
	}

	private Vector3 CalculateHitDirectionFromPlayer(Vector3 hitPoint) { return (hitPoint - _transform.position).normalized; }

	public void UpdateRateOfFire(int newRate)
	{
		_lastRateOfFire = _rateOfFire;
		_rateOfFire = newRate;
	}
}

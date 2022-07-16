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
	private float _nextTimeToFire;
	private int _lastRateOfFire;

	[Header("LOCAL REFERENCES")]
	[SerializeField] private Transform _meshTransorm;
	[SerializeField] private PlayerData _playerData;

    private Transform _rootTransform;
	private Transform _camera;

	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = this.transform; }
		if (_meshTransorm == null) { _meshTransorm = transform.GetChild(0); }
		if (_playerData == null) { _playerData = GetComponent<PlayerData>(); }
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
			Quaternion rotation = CalculateRotationAngle(hit.point);
			rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
			_meshTransorm.rotation = rotation;

			if (hit.collider.tag != "Player")
				Shoot(CalculateHitDirectionFromPlayer(new Vector3(hit.point.x, 1, hit.point.z)));
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

		_rootTransform.Translate(moveDirection * _speed * Time.deltaTime);
	}

	private void Shoot(Vector3 direction)
	{
		if ((Input.GetAxis("Fire1") > 0) && (_nextTimeToFire <= 0))
		{
			Projectile instance = Instantiate<GameObject>(_projectilePrefab).GetComponent<Projectile>();
			instance.Initialize(_meshTransorm.position, direction, _playerData.damage);

			_nextTimeToFire = 1 / (float)_rateOfFire;
		}
	}

	private Quaternion CalculateRotationAngle(Vector3 hitPoint)
	{
		return Quaternion.LookRotation(CalculateHitDirectionFromPlayer(hitPoint));
	}

	private Vector3 CalculateHitDirectionFromPlayer(Vector3 hitPoint)
	{
		Vector3 direction = (hitPoint - _meshTransorm.position).normalized;
		return direction;
	}

	public void UpdateRateOfFire(int newRate)
	{
		_lastRateOfFire = _rateOfFire;
		_rateOfFire = newRate;
	}
}

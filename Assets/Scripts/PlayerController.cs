using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("DATA")]
	public PlayerData data;

	[Header("SHOOTING")]
	[SerializeField] private GameObject _projectilePrefab;
	[Space]
	private float _nextTimeToFire;

	[Header("AUDIO")]
	[SerializeField] private AudioSource _audioSource;

	[Header("LOCAL REFERENCES")]
	[SerializeField] private Transform _meshTransorm;

    private Transform _rootTransform;
	private Transform _camera;
    private float _lowestHeight;

	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = this.transform; }
		if (_meshTransorm == null) { _meshTransorm = transform.GetChild(0);}
		if (data == null) { data = GetComponent<PlayerData>(); }
		if (_camera == null) { _camera = Camera.main.transform; }

        _lowestHeight = _meshTransorm.position.y;
	}

    void Update()
    {
		Move();
        Jump();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			Quaternion rotation = CalculateRotationAngle(hit.point);
			rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
			_rootTransform.rotation = rotation;

			if (hit.collider.tag != "Player")
				Shoot(CalculateHitDirectionFromPlayer(new Vector3(hit.point.x, 1, hit.point.z)));
		}

		_nextTimeToFire -= Time.deltaTime;
		if (_nextTimeToFire < 0)
			_nextTimeToFire = 0;
    }

	private void Move()
	{
		float vertical = Input.GetAxisRaw("Vertical");
		Vector3 moveDirection = Vector3.zero;

		if (vertical != 0)
			moveDirection.z = vertical; // We set the Z-axis because we are top-down

		_rootTransform.Translate(moveDirection * data.speed * Time.deltaTime);
	}

	private void Shoot(Vector3 direction)
	{
		if ((Input.GetAxis("Fire1") > 0) && (_nextTimeToFire <= 0))
		{
            for (int i=0; i < data.projectileAmount; i++) {
                Projectile instance = Instantiate<GameObject>(_projectilePrefab, StaticReferences.Instance.projectileContainer).GetComponent<Projectile>();
                var position = _meshTransorm.position;
                var relative_position = Vector3.forward;

                relative_position = (_meshTransorm.rotation * relative_position).normalized * i;


                instance.Initialize(position + relative_position, direction, data.damage);
                instance.transform.localScale *= (data.projectileRadiusBonus+1);
                instance.speed += data.projectileSpeedBonus; 
            }

			// We play the audio source ("Pew")
			_audioSource.pitch = Random.Range(0.9f, 1.1f);
			_audioSource.Play();

			_nextTimeToFire = 1 / (float)data.fireRate;
		}
	}

    //TODO fix bug trail getting too far (probably due to the isKinematic?)
    private void Jump() {
        if (Input.GetKeyDown("space")) {
            if (data.jumpAmount > 0) {
                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);

                data.jumpAmount -= 1;
            }
        }

        if (_meshTransorm.position.y < _lowestHeight) {
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            var rot = _meshTransorm.rotation;
            var pos = _meshTransorm.position;
            pos.y = _lowestHeight;
            _meshTransorm.SetPositionAndRotation(pos, rot);
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

	public void UpdateFireRate(int newRate)
	{
		data.fireRate = newRate;
	}
}

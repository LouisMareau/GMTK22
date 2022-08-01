using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("DATA")]
	public PlayerData data;

	[Header("SHOOTING")]
	[SerializeField] private GameObject _prefabProjectileStandard;
	[SerializeField] private GameObject _prefabProjectileSeeker;
	private float _nextTimeToFireStandard;
	private float _nextTimeToFireSeeker;

	[Header("VFX")]
	[SerializeField] private GameObject _prefabVFX_hitSparks;
	[SerializeField] private GameObject _prefabVFX_death;

	[Header("AUDIO")]
	[SerializeField] private AudioSource _audioSource;

	[Header("LOCAL REFERENCES")]
    private Transform _rootTransform;
	private Transform _meshTransform;
	private Transform _camera;

	private void Awake()
	{
		if (_rootTransform == null) { _rootTransform = this.transform; }
		if (_meshTransform == null) { _meshTransform = transform.GetChild(0); }
		if (data == null) { data = GetComponent<PlayerData>(); }
		if (_camera == null) { _camera = Camera.main.transform; }
	}

    void Update()
    {
		if (GameManager.IsPlaying)
		{
			Move();

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				Quaternion rotation = CalculateRotationAngle(hit.point);
				rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
				_rootTransform.rotation = rotation;

				if ((hit.collider.tag != "Player") && (data.status != PlayerData.PlayerStatus.CANNOT_SHOOT))
					Shoot(CalculateHitDirectionFromPlayer(new Vector3(hit.point.x, 1, hit.point.z)));
			}

			_nextTimeToFireStandard -= Time.deltaTime;
			_nextTimeToFireSeeker -= Time.deltaTime;
			if (_nextTimeToFireStandard < 0) _nextTimeToFireStandard = 0;
			if (_nextTimeToFireSeeker < 0) _nextTimeToFireSeeker = 0;
		}
    }

	private void Move()
	{
		float vertical = Input.GetAxis("Vertical");
		Vector3 moveDirection = Vector3.zero;

		if (vertical != 0)
			moveDirection.z = vertical; // We set the Z-axis because we are top-down

		_rootTransform.Translate(moveDirection * data.speed * Time.deltaTime);
	}

	private void Shoot(Vector3 direction)
	{
		if ((Input.GetAxis("Fire1") > 0))
		{
			if (data.finalFireRateStandard > 0 && _nextTimeToFireStandard <= 0)
			{
				Projectile instance = Instantiate<GameObject>(_prefabProjectileStandard, StaticReferences.Instance.projectileContainer).GetComponent<Projectile>();
				instance.Initialize(_meshTransform.position, direction, data.finalDamage);

				_nextTimeToFireStandard = 1 / data.finalFireRateStandard;
			}

			if (data.finalFireRateSeeker > 0 && _nextTimeToFireSeeker <= 0)
			{
				Projectile instance = Instantiate<GameObject>(_prefabProjectileSeeker, StaticReferences.Instance.projectileContainer).GetComponent<Projectile>();
				instance.Initialize(_meshTransform.position, direction, data.finalDamage);

				_nextTimeToFireSeeker = 1 / data.finalFireRateSeeker;
			}

			// We play the audio source ("Pew")
            if (!_audioSource.isPlaying || _audioSource.time > 0.23f) {
                _audioSource.pitch = Random.Range(0.9f, 1.1f);
                _audioSource.Play();
            }
		}
	}

	private Quaternion CalculateRotationAngle(Vector3 hitPoint)
	{
		return Quaternion.LookRotation(CalculateHitDirectionFromPlayer(hitPoint));
	}

	private Vector3 CalculateHitDirectionFromPlayer(Vector3 hitPoint)
	{
		Vector3 direction = (hitPoint - _meshTransform.position).normalized;
		return direction;
	}

	public void Kill()
	{
		PlayAnim_Death();
		Destroy(gameObject);
	}

	#region ANIMATIONS / VFX
	public void PlayAnim_Spawn()
	{
		// VFX: Spawn
		// [TO DO] ...
	}

	public void PlayAnim_HitSparks()
	{
		// VFX: Hit Sparks
		Instantiate(_prefabVFX_hitSparks, _meshTransform.position, _prefabVFX_hitSparks.transform.rotation, StaticReferences.Instance.vfxContainer);
	}

	public void PlayAnim_Death()
	{
		// VFX: Death
		Instantiate(_prefabVFX_death, _meshTransform.position, _prefabVFX_death.transform.rotation, StaticReferences.Instance.vfxContainer);
	}
	#endregion

	// -------------------------------------------------------------------------------
	// [TO DO] Add dash mechanic
	// -------------------------------------------------------------------------------
}

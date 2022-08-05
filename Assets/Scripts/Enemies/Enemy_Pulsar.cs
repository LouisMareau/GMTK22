using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pulsar : Enemy
{
	[Header("PULSAR")]
	[SerializeField] private float minDistanceFromPlayer = 30f;
	[Space]
	[SerializeField] private float _siegeDuration = 0.7f;
	[SerializeField] private float _chargeDuration = 3.5f;
	[SerializeField] private float _holdDuration = 0.3f;
	[SerializeField] private float _blastDuration = 2.0f;
	[SerializeField] private float _cooldownDuration = 6.0f;
	[Space]
	[SerializeField] private AnimationCurve _blastSizeOverTime;

	[Header("PULSAR VFXs")]
	[SerializeField] private GameObject _prefabVFX_targetingLaser;
	[SerializeField] private GameObject _prefabVFX_blast;
	private GameObject _targetingLaserInstance;
	private GameObject _blastInstance;

	[Header("COMPONENTS")]
	[SerializeField] private EnemyFeature_EnergyRing _energyRing;

	private bool _isAttacking = false;
	private bool _canRotate = true;

	protected override void Update()
	{
		if (_playerTransform == null) return;

		if (GameManager.IsPlaying)
		{
			currentDirection = CalculateDirection(_playerTransform.position);
			distanceFromPlayer = CalculateDistanceFromPlayer();

			if (!_isAttacking)
			{
				if (CameraHelper.IsWithinBounds(_rootTransform.position))
					if (IsInRangeForPulsarCharge())
					{
						StartCharging();
						_isAttacking = true;
						_canRotate = false;
					}

				if (IsTooFarFromPlayer())
					MoveTowards(currentDirection, speed);
			}

			if (_canRotate)
				RotateTowards(_playerTransform.position, _rotationalSpeed);
		}
	}

	#region GAMEPLAY
	private bool IsTooFarFromPlayer() { return distanceFromPlayer > minDistanceFromPlayer; }
	private bool IsInRangeForPulsarCharge() { return distanceFromPlayer <= minDistanceFromPlayer; }

	private void StartCharging() { StartCoroutine(StartCharging_Coroutine()); }
	private IEnumerator StartCharging_Coroutine()
	{
		Vector3 blastDirection = currentDirection;

		_energyRing.StartSieging(_siegeDuration);
		_energyRing.StartEnergyFlow(_chargeDuration + 0.5f);

		PlayAnim_TargetingLaser();

		float timer = 0;
		while (timer < _chargeDuration)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		LaunchBlast(blastDirection);
	}

	private void LaunchBlast(Vector3 blastDirection) { StartCoroutine(LaunchBlast_Coroutine(blastDirection)); }
	private IEnumerator LaunchBlast_Coroutine(Vector3 blastDirection)
	{
		Ray ray = new Ray(_meshTransform.position, blastDirection);
		RaycastHit hit;

		// Short hold before firing
		float timer = 0.0f;
		while (timer <= _holdDuration)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		PlayAnim_Blast();
		_energyRing.StopEnergyFlow(_blastDuration + _cooldownDuration);

		// launch blast
		timer = 0.0f;
		while (timer <= _blastDuration)
		{
			if (GameManager.IsPlaying)
			{
				// We raycast straight forward
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.CompareTag("Player"))
					{
						PlayerController player = hit.collider.GetComponent<PlayerController>();
						player.data.LoseLife(damage);
					}
				}

				timer += Time.deltaTime;
			}

			yield return null;
		}

		SetCooldown();
	}

	private void SetCooldown() { StartCoroutine(SetCooldown_Coroutine()); }
	private IEnumerator SetCooldown_Coroutine()
	{
		_energyRing.StopSieging(_cooldownDuration / 5.0f);

		// We can reset to the base behaviour
		_canRotate = true;

		float timer = 0;
		while (timer <= _cooldownDuration)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}
		
		_isAttacking = false;
	}

	protected override void Kill(float delay = 0)
	{
		GameRecords.enemyPulsarKilled++;

		if (_targetingLaserInstance != null)
			Destroy(_targetingLaserInstance);

		base.Kill(delay);
	}
	#endregion

	#region COLLISION EVENTS
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();
			player.data.LoseLife(damage);
			player.PlayAnim_HitSparks();

			Kill();
		}
	}
	#endregion

	#region ANIMATIONS / VFX
	private void PlayAnim_TargetingLaser() { StartCoroutine(PlayAnim_TargetingLaser_Coroutine()); }
	private IEnumerator PlayAnim_TargetingLaser_Coroutine()
	{
		// VFX: Targeting Laser
		_targetingLaserInstance = Instantiate(_prefabVFX_targetingLaser, _meshTransform.position, _meshTransform.rotation * _prefabVFX_targetingLaser.transform.rotation, StaticReferences.Instance.vfxContainer);
		LineRenderer lr = _targetingLaserInstance.GetComponentInChildren<LineRenderer>();

		float timer = 0;
		while (timer < _chargeDuration)
		{
			Vector3 laser = new Vector3(0, 0, Mathf.Lerp(0.0f, 80.0f, timer / _chargeDuration));
			lr.SetPosition(1, laser);

			timer += Time.deltaTime;
			yield return null;
		}

		Destroy(_targetingLaserInstance);
	}

	private void PlayAnim_Blast() { StartCoroutine(PlayAnim_Blast_Coroutine()); }
	private IEnumerator PlayAnim_Blast_Coroutine()
	{
		// VFX: Targeting Laser
		_blastInstance = Instantiate(_prefabVFX_blast, _meshTransform.position, _meshTransform.rotation * _prefabVFX_blast.transform.rotation, StaticReferences.Instance.vfxContainer);
		LineRenderer lr = _blastInstance.GetComponentInChildren<LineRenderer>();

		float timer = 0;
		while (timer < _blastDuration)
		{
			lr.widthMultiplier = _blastSizeOverTime.Evaluate(timer / _blastDuration) + Random.Range(0.9f, 1.1f);

			timer += Time.deltaTime;
			yield return null;
		}

		Destroy(_blastInstance);
	}
	#endregion
}
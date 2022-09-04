using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pulsar : Enemy
{
	public float siegeDuration { get; private set; }
	public float chargeDuration { get; private set; }
	public float holdDuration { get; private set; }
	public float blastDuration { get; private set; }
	public float cooldownDuration { get; private set; }

	[Header("PULSAR GENERAL")]
	[SerializeField] private float minDistanceFromPlayer = 30f;
	[SerializeField] private float delayBeforeReady = 2.0f;
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

	#region INITIALIZATION
	protected override void Awake()
	{
		base.Awake();

		BaseDataManager.BaseData_Enemy_Pulsar baseData = BaseDataManager.baseDataEnemy_Pulsar;
		health = baseData.health;
		speed = baseData.speed;
		damage = baseData.damage;
		scoreWhenKilled = baseData.scoreWhenKilled;
		rotationalSpeed = baseData.rotationalSpeed;
		chargeDuration = baseData.chargeDuration;
		holdDuration = baseData.holdDuration;
		blastDuration = baseData.blastDuration;
		cooldownDuration = baseData.cooldownDuration;
		siegeDuration = chargeDuration / 4.0f;
	}
	#endregion

	protected override void Update()
	{
		if (_playerTransform == null) return;

		if (GameManager.IsPlaying)
		{
			currentDirection = CalculateDirection(_playerTransform.position);
			distanceFromPlayer = CalculateDistanceFromPlayer();

			if (!_isAttacking)
			{
				if (IsReady() && CameraHelper.IsWithinBounds(_rootTransform.position))
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
				RotateTowards(_playerTransform.position, rotationalSpeed);
		}
	}

	public void UpdateData(float health, float speed, int damage, int scoreWhenKilled, float rotationalSpeed, float chargeDuration, float holdDuration, float blastDuration, float cooldownDuration)
	{
		this.health = health;
		this.speed = speed;
		this.damage = damage;
		this.scoreWhenKilled = scoreWhenKilled;
		this.rotationalSpeed = rotationalSpeed;
		this.chargeDuration = chargeDuration;
		this.holdDuration = holdDuration;
		this.blastDuration = blastDuration;
		this.cooldownDuration = cooldownDuration;
		this.siegeDuration = chargeDuration / 4.0f;

		HUDManager.overlayData.enemyPHealth.Set("Health", health);
		HUDManager.overlayData.enemyPSpeed.Set("Speed", speed);
		HUDManager.overlayData.enemyPDamage.Set("Damage", damage);
		HUDManager.overlayData.enemyPScoreOnKill.Set("Score On Kill", scoreWhenKilled);
		HUDManager.overlayData.enemyPRotationSpeed.Set("Rotation Speed", rotationalSpeed);
		HUDManager.overlayData.enemyPChargeDuration.Set("Charge Duration", chargeDuration);
		HUDManager.overlayData.enemyPHoldDuration.Set("Hold Duration", holdDuration);
		HUDManager.overlayData.enemyPBlastDuration.Set("Blast Duration", blastDuration);
		HUDManager.overlayData.enemyPCooldownDuration.Set("Cooldown Duration", cooldownDuration);
	}

	#region GAMEPLAY
	private bool IsTooFarFromPlayer() { return distanceFromPlayer > minDistanceFromPlayer; }
	private bool IsInRangeForPulsarCharge() { return distanceFromPlayer <= minDistanceFromPlayer; }
	private bool IsReady() { return GameManager.timeSinceStart >= _spawnTime + delayBeforeReady; }

	private void StartCharging() { StartCoroutine(StartCharging_Coroutine()); }
	private IEnumerator StartCharging_Coroutine()
	{
		Vector3 blastDirection = currentDirection;

		_energyRing.StartSieging(siegeDuration);
		_energyRing.StartEnergyFlow(chargeDuration + 0.5f);

		PlayAnim_TargetingLaser();

		float timer = 0;
		while (timer < chargeDuration)
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
		while (timer <= holdDuration)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		PlayAnim_Blast();
		_energyRing.StopEnergyFlow(blastDuration + cooldownDuration);

		// launch blast
		timer = 0.0f;
		while (timer <= blastDuration)
		{
			if (GameManager.IsPlaying)
			{
				// We raycast straight forward
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.CompareTag("Player"))
					{
						HealthManager.Instance.LoseHealth(damage);

						Destroy(_blastInstance);
						break;
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
		_energyRing.StopSieging(cooldownDuration / 5.0f);

		// We can reset to the base behaviour
		_canRotate = true;

		float timer = 0;
		while (timer <= cooldownDuration)
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

		if (_blastInstance != null)
			Destroy(_blastInstance);

		base.Kill(delay);
	}
	#endregion

	#region COLLISION EVENTS
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();
			HealthManager.Instance.LoseHealth(damage);
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
		while (timer < chargeDuration)
		{
			if (GameManager.IsGameOver)
				break;

			Vector3 laser = new Vector3(0, 0, Mathf.Lerp(0.0f, 80.0f, timer / chargeDuration));
			lr.SetPosition(1, laser);

			if (GameManager.IsPlaying)
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
		while (timer < blastDuration)
		{
			if (GameManager.IsGameOver)
				break;

			lr.widthMultiplier = _blastSizeOverTime.Evaluate(timer / blastDuration) + Random.Range(0.9f, 1.1f);

			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		Destroy(_blastInstance);
	}
	#endregion
}
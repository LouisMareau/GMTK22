using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DifficultyScalingManager : MonoBehaviour
{
	#region INTERNAL CLASSES
	[System.Serializable]
	public class DifficultyScalingData
	{
		[HideInInspector] public string name;
		
		public float phaseStartTime;

		[Header("Enemies - MELEE DETONATOR")]
		public Vector2Int rangeValueMeleeDetonatorHealth;
		public Vector2Int rangeValueMeleeDetonatorDamage;
		public Vector2Int rangeValueMeleeDetonatorSpeed;
		public Vector2Int rangeValueMeleeDetonatorScore;
		public Vector2Int rangeValueMeleeDetonatorRotationalSpeed;
		[Space]
		public Vector2 rangeMultiplierMeleeDetonatorSpawnInterval;
		
		[Header("Enemies - DIE HOLDER")]
		public Vector2Int rangeValueDieHolderHealth;
		public Vector2Int rangeValueDieHolderDamage;
		public Vector2Int rangeValueDieHolderSpeed;
		public Vector2Int rangeValueDieHolderScore;
		public Vector2Int rangeValueDieHolderRotationalSpeed;
		[Space]
		public Vector2 rangeMultiplierDieHolderSpawnInterval;

		[Header("Enemies - PULSAR")]
		public Vector2Int rangeValuePulsarHealth;
		public Vector2Int rangeValuePulsarDamage;
		public Vector2Int rangeValuePulsarSpeed;
		public Vector2Int rangeValuePulsarScore;
		public Vector2Int rangeValuePulsaRotationalSpeed;
		[Space]
		public Vector2 rangeMultiplierPulsarSpawnInterval;
		[Space]
		public Vector2 rangeMultiplierPulsarChargeDuration;
		public Vector2 rangeMultiplierPulsarHoldDuration;
		public Vector2 rangeMultiplierPulsarBlastDuration;
		public Vector2 rangeMultiplierPulsarCooldownDuration;

		public void ApplyDifficulty()
		{
			// Setting random new values
			int randomValueHealthMD = Random.Range(rangeValueMeleeDetonatorHealth.x, rangeValueMeleeDetonatorHealth.y);
			int randomValueDamageMD = Random.Range(rangeValueMeleeDetonatorDamage.x, rangeValueMeleeDetonatorDamage.y);
			int randomValueSpeedMD = Random.Range(rangeValueMeleeDetonatorSpeed.x, rangeValueMeleeDetonatorSpeed.y);
			int randomValueScoreMD = Random.Range(rangeValueMeleeDetonatorScore.x, rangeValueMeleeDetonatorScore.y);
			int randomValueRotationalSpeedMD = Random.Range(rangeValueMeleeDetonatorRotationalSpeed.x, rangeValueMeleeDetonatorRotationalSpeed.y);
			int randomValueHealthDH = Random.Range(rangeValueDieHolderHealth.x, rangeValueDieHolderHealth.y);
			int randomValueDamageDH = Random.Range(rangeValueDieHolderDamage.x, rangeValueDieHolderDamage.y);
			int randomValueSpeedDH = Random.Range(rangeValueDieHolderSpeed.x, rangeValueDieHolderSpeed.y);
			int randomValueScoreDH = Random.Range(rangeValueDieHolderScore.x, rangeValueDieHolderScore.y);
			int randomValueRotationalSpeedDH = Random.Range(rangeValueDieHolderRotationalSpeed.x, rangeValueDieHolderRotationalSpeed.y);
			int randomValueHealthP = Random.Range(rangeValuePulsarHealth.x, rangeValuePulsarHealth.y);
			int randomValueDamageP = Random.Range(rangeValuePulsarDamage.x, rangeValuePulsarDamage.y);
			int randomValueSpeedP = Random.Range(rangeValuePulsarSpeed.x, rangeValuePulsarSpeed.y);
			int randomValueScoreP = Random.Range(rangeValuePulsarScore.x, rangeValuePulsarScore.y);
			int randomValueRotationalSpeedP = Random.Range(rangeValuePulsaRotationalSpeed.x, rangeValuePulsaRotationalSpeed.y);
			float randomMultiplierChargeDurationP = Random.Range(rangeMultiplierPulsarChargeDuration.x, rangeMultiplierPulsarChargeDuration.y);
			float randomMultiplierHoldDurationP = Random.Range(rangeMultiplierPulsarHoldDuration.x, rangeMultiplierPulsarHoldDuration.y);
			float randomMultiplierBlastDurationP = Random.Range(rangeMultiplierPulsarBlastDuration.x, rangeMultiplierPulsarBlastDuration.y);
			float randomMultiplierCooldownDurationP = Random.Range(rangeMultiplierPulsarCooldownDuration.x, rangeMultiplierPulsarCooldownDuration.y);

			// -----------------------------------
			// Melee Detonator
			BaseDataManager.BaseData_Enemy_MeleeDetonator baseDataMD = BaseDataManager.baseDataEnemy_MeleeDetonator;
			baseDataMD.health += randomValueHealthMD;
			baseDataMD.speed += randomValueSpeedMD;
			baseDataMD.damage += randomValueDamageMD;
			baseDataMD.scoreWhenKilled += randomValueScoreMD;
			baseDataMD.rotationalSpeed += randomValueRotationalSpeedMD;
			baseDataMD.UpdateData(
				baseDataMD.health,
				baseDataMD.speed,
				baseDataMD.damage,
				baseDataMD.scoreWhenKilled,
				baseDataMD.rotationalSpeed
			);
			EnemySpawner.Instance.GetEnemiesByType(EnemyType.MELEE_DETONATOR).ForEach(enemy =>
			{
				enemy.UpdateData(
					baseDataMD.health,
					baseDataMD.speed,
					baseDataMD.damage,
					baseDataMD.scoreWhenKilled,
					baseDataMD.rotationalSpeed
				);
			});

			// -----------------------------------
			// Die Holder
			BaseDataManager.BaseData_Enemy_DieHolder baseDataDH = BaseDataManager.baseDataEnemy_DieHolder;
			baseDataDH.health += randomValueHealthDH;
			baseDataDH.speed += randomValueSpeedDH;
			baseDataDH.damage += randomValueDamageDH;
			baseDataDH.scoreWhenKilled += randomValueScoreDH;
			baseDataDH.rotationalSpeed += randomValueRotationalSpeedDH;
			baseDataDH.UpdateData(
				baseDataDH.health,
				baseDataDH.speed,
				baseDataDH.damage,
				baseDataDH.scoreWhenKilled,
				baseDataDH.rotationalSpeed
			);
			EnemySpawner.Instance.GetEnemiesByType(EnemyType.DIE_HOLDER).ForEach(enemy =>
			{
				enemy.UpdateData(
					baseDataDH.health,
					baseDataDH.speed,
					baseDataDH.damage,
					baseDataDH.scoreWhenKilled,
					baseDataDH.rotationalSpeed
				);
			});

			// -----------------------------------
			// Pulsar
			BaseDataManager.BaseData_Enemy_Pulsar baseDataP = BaseDataManager.baseDataEnemy_Pulsar;
			baseDataP.health += randomValueHealthP;
			baseDataP.speed += randomValueSpeedP;
			baseDataP.damage += randomValueDamageP;
			baseDataP.scoreWhenKilled += randomValueScoreP;
			baseDataP.rotationalSpeed += randomValueRotationalSpeedP;
			baseDataP.chargeDuration *= randomMultiplierChargeDurationP;
			baseDataP.holdDuration *= randomMultiplierHoldDurationP;
			baseDataP.blastDuration *= randomMultiplierBlastDurationP;
			baseDataP.cooldownDuration *= randomMultiplierCooldownDurationP;
			baseDataP.UpdateData(
				baseDataP.health,
				baseDataP.speed,
				baseDataP.damage,
				baseDataP.scoreWhenKilled,
				baseDataP.rotationalSpeed,
				baseDataP.chargeDuration,
				baseDataP.holdDuration,
				baseDataP.blastDuration,
				baseDataP.cooldownDuration
			);

			EnemySpawner.Instance.GetEnemiesAsPulsar().ForEach(enemy =>
			{
				enemy.UpdateData(
					baseDataP.health,
					baseDataP.speed,
					baseDataP.damage,
					baseDataP.scoreWhenKilled,
					baseDataP.rotationalSpeed,
					baseDataP.chargeDuration,
					baseDataP.holdDuration,
					baseDataP.blastDuration,
					baseDataP.cooldownDuration
				);
			});

			EnemySpawner.Instance.spawnTypes.ForEach(spawnType => {
				switch (spawnType.type)
				{
					case EnemyType.MELEE_DETONATOR:
						spawnType.intervalRange *= Random.Range(rangeMultiplierMeleeDetonatorSpawnInterval.x, rangeMultiplierMeleeDetonatorSpawnInterval.y);
						break;
					case EnemyType.DIE_HOLDER:
						spawnType.intervalRange *= Random.Range(rangeMultiplierDieHolderSpawnInterval.x, rangeMultiplierDieHolderSpawnInterval.y);
						break;
					case EnemyType.PULSAR:
						spawnType.intervalRange *= Random.Range(rangeMultiplierPulsarSpawnInterval.x, rangeMultiplierPulsarSpawnInterval.y);
						break;
					default:
						break;
				}
			});
		}
	}
	#endregion

	public List<DifficultyScalingData> difficultyScales;

	public DifficultyScalingData nextDifficulty;
	public int nextDifficultyIndex = 1;

	#region IN-EDITOR
	public void OnValidate()
	{
		for (int i = 0; i < difficultyScales.Count; i++)
			difficultyScales[i].name = $"Difficulty +{ i + 1 } : { difficultyScales[i].phaseStartTime }s";
	}
	#endregion

	#region INITIALIZATION
	private void Awake()
	{
		if (nextDifficultyIndex != 1) { nextDifficultyIndex = 1; }
	}

	private void Start()
	{
		nextDifficulty = difficultyScales[nextDifficultyIndex];

		StartIncreaseDifficulty();
	}
	#endregion

	#region RUNTIME

	private void StartIncreaseDifficulty() { StartCoroutine(StartIncreaseDifficulty_Coroutine()); }
	private IEnumerator StartIncreaseDifficulty_Coroutine()
	{
		while (true)
		{
			if (GameManager.IsPlaying)
			{
				if (GameManager.timeSinceStart > nextDifficulty.phaseStartTime)
				{
					nextDifficulty.ApplyDifficulty();

					nextDifficultyIndex++;
					nextDifficulty = difficultyScales[nextDifficultyIndex];
				}

				yield return null;
			}
			else yield return null;
		}
	}
	#endregion
}
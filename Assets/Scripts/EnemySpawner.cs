using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	#region SINGLETON
	public static EnemySpawner Instance { get; private set; }
	#endregion

	public float baseTankyEnemySpawnInterval = 25f;
	public float tankyEnemySpawnInterval;
	private float _timeBeforeNextTankyEnemy;
	[Space]
	public Vector2 baseSpawnRandomDelay = new Vector2(0.5f, 2.5f);
	public Vector2 spawnRandomDelay;
	public float spawnRadius = 80f;
	public float spawnPlayerDetectionRadius = 3f;
	[Space]
	public List<GameObject> enemyTypesPrefabs;
	public Dictionary<EnemyType, GameObject> enemyPrefabs = new Dictionary<EnemyType, GameObject>();

	private Transform _transform;

	private void Awake()
	{
		Instance = this;

		if (_transform == null) { _transform = transform; }

		tankyEnemySpawnInterval = baseTankyEnemySpawnInterval;
		spawnRandomDelay = baseSpawnRandomDelay;

		_timeBeforeNextTankyEnemy = tankyEnemySpawnInterval;

		foreach (GameObject enemy in enemyTypesPrefabs)
			enemyPrefabs.Add(enemy.GetComponent<Enemy>().type, enemy);
	}

	private void Start()
	{
		StartSpawning();
	}

	private void Update()
	{
		if (IsTankyEnemyReadyToSpawn())
		{
			SpawnEnemy(EnemyType.TANK);
			_timeBeforeNextTankyEnemy = tankyEnemySpawnInterval;
		}

		_timeBeforeNextTankyEnemy -= Time.deltaTime;
		if (_timeBeforeNextTankyEnemy < 0)
			_timeBeforeNextTankyEnemy = 0;
	}

	private void StartSpawning() { StartCoroutine(StartSpawning_Coroutine()); }
	private IEnumerator StartSpawning_Coroutine()
	{
		while(true)
		{
			float randomDelay = Random.Range(spawnRandomDelay.x, spawnRandomDelay.y);
			yield return new WaitForSeconds(randomDelay);

			SpawnEnemy(EnemyType.SMALL);
		}
	}

	private bool IsPlayerInDetectionRadius(Vector3 targetSpawnLocation)
	{
		Collider[] colliders = Physics.OverlapSphere(targetSpawnLocation, spawnPlayerDetectionRadius);
		foreach (Collider collider in colliders)
			if (collider.tag == "Player")
				return true;

		return false;
	}
	private void SpawnEnemy(EnemyType type)
	{
		Vector3 randomLocation;
		do
		{
			Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
			randomLocation = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y);
		}
		while (IsPlayerInDetectionRadius(randomLocation));

		GameObject instance = Instantiate<GameObject>(enemyPrefabs[type], randomLocation, enemyPrefabs[type].transform.rotation, StaticReferences.Instance.enemyContainer);
	}

	public void IncreaseDifficulty(float spawnMultiplier, float enemyStatMultiplier)
	{
		tankyEnemySpawnInterval *= spawnMultiplier;
		spawnRandomDelay *= spawnMultiplier;

		foreach (GameObject prefab in enemyPrefabs.Values)
			prefab.GetComponent<Enemy>().IncreaseDifficulty(enemyStatMultiplier);
	}

	private bool IsTankyEnemyReadyToSpawn()
	{
		return _timeBeforeNextTankyEnemy <= 0;
	}
}
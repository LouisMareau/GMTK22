using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public float tankyEnemySpawnInterval = 25f;
	private float _timeBeforeNextTankyEnemy;
	[Space]
	public Vector2 spawnRandomDelay = new Vector2(0.5f, 5f);
	public float spawnRadius = 80f;
	public float spawnPlayerDetectionRadius = 3f;
	[Space]
	public List<GameObject> enemyTypesPrefabs;
	public Dictionary<EnemyType, GameObject> enemyPrefabs = new Dictionary<EnemyType, GameObject>();

	private Transform _transform;

	private void Awake()
	{
		if (_transform == null) { _transform = transform; }

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

		GameObject instance = Instantiate<GameObject>(enemyPrefabs[type], randomLocation, enemyPrefabs[type].transform.rotation, _transform);
	}

	private bool IsTankyEnemyReadyToSpawn()
	{
		return _timeBeforeNextTankyEnemy <= 0;
	}
}
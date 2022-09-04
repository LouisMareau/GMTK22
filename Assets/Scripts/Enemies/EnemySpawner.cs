using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class EnemySpawner : MonoBehaviour
{
	#region SINGLETON
	public static EnemySpawner Instance { get; private set; }
	#endregion

	#region INTERNAL CLASSES
	[System.Serializable]
	public class EnemySpawnType
	{
		public EnemyType type;
		public GameObject prefab;
		[Space]
		public float delayBeforeStart;
		public Vector2 intervalRange;
		[Space]
		[HideInInspector] public float timeLeftBeforeSpawn;

		public bool IsReadyToSpawn() { return (GameManager.timeSinceStart >= delayBeforeStart) && (timeLeftBeforeSpawn <= 0.0f); }
		public void UpdateTimeBeforeSpawn() { timeLeftBeforeSpawn -= Time.deltaTime; }
		public void ResetTimeBeforeSpawn() { timeLeftBeforeSpawn = Random.Range(intervalRange.x, intervalRange.y); }
	}
	#endregion

	[Header("COLLECTIONS")]
	public List<EnemySpawnType> spawnTypes;
	public List<Enemy> Enemies { get; private set; }

	[Header("SPAWN DATA")]
	public float spawnRadius = 80f;
	public float spawnPlayerDetectionRadius = 3f;

	private Transform _rootTransform;


	private void Awake()
	{
		Instance = this;

		if (Enemies == null) { Enemies = new List<Enemy>(); }

		if (_rootTransform == null) { _rootTransform = transform; }

		InitSpawnTypes();
	}

	private void InitSpawnTypes()
	{
		foreach (EnemySpawnType enemy in spawnTypes)
		{
			enemy.ResetTimeBeforeSpawn();
			HUDManager.UpdateEnemyRandomSpawnInterval(enemy.type, enemy.intervalRange);
		}
	}

	private void Start()
	{
		StartSpawning();
	}

	private void StartSpawning() { StartCoroutine(StartSpawning_Coroutine()); }
	private IEnumerator StartSpawning_Coroutine()
	{
		while (true)
		{
			if (IsPlaying)
			{
				foreach (EnemySpawnType type in spawnTypes)
				{
					type.UpdateTimeBeforeSpawn();

					if (type.IsReadyToSpawn())
					{
						SpawnEnemy(type);

						// We reset the interval
						type.ResetTimeBeforeSpawn();
					}
				}
			}

			yield return null;
		}
	}

	private bool IsPlayerInDetectionRadius(Vector3 targetSpawnLocation)
	{
		Collider[] colliders = Physics.OverlapSphere(targetSpawnLocation, spawnPlayerDetectionRadius);

		foreach (Collider collider in colliders)
			if (collider.CompareTag("Player"))
				return true;

		return false;
	}
	private void SpawnEnemy(EnemySpawnType type)
	{
		Vector3 randomLocation;
		do
		{
			Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
			randomLocation = new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y);
		}
		while (IsPlayerInDetectionRadius(randomLocation));

		GameObject instance = Instantiate<GameObject>(type.prefab, randomLocation, type.prefab.transform.rotation, StaticReferences.Instance.enemyContainer);
        Enemies.Add(instance.GetComponent<Enemy>());
	}

    public void RemoveEnemy(Enemy enemy) { Enemies.Remove(enemy); }

    public Enemy FindClosestEnemy(Vector3 position)
	{
		if (Enemies.Count == 0) { return null; }

        var closestEnemy = Enemies[0];
        var closestDistance = Vector3.Distance(closestEnemy.transform.position, position);

        for (int i = 1; i < Enemies.Count; i++)
		{
            var distance = Vector3.Distance(Enemies[i]._rootTransform.position, position);
            if (closestDistance > distance)
			{
                closestDistance = distance;
                closestEnemy = Enemies[i];
            }
        } 

        return closestEnemy;
    }

	#region QUALITY OF LIFE
	public List<Enemy> GetEnemiesByType(EnemyType type)
	{
		List<Enemy> enemies = new List<Enemy>();
		foreach (Enemy enemy in Enemies)
		{
			if (enemy.type == type)
				enemies.Add(enemy);
		}

		return enemies;
	}

	public List<Enemy_Pulsar> GetEnemiesAsPulsar()
	{
		List<Enemy_Pulsar> enemies = new List<Enemy_Pulsar>();
		foreach (Enemy enemy in Enemies)
		{
			if (enemy.type == EnemyType.PULSAR)
				enemies.Add((Enemy_Pulsar)enemy);
		}

		return enemies;
	}
	#endregion
}

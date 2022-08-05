using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	#region SINGLETON
	public static EnemySpawner Instance { get; private set; }
	#endregion

	#region INTERNAL CLASSES
	[System.Serializable]
	public class EnemySpawnType
	{
		public string name;
		public GameObject prefab;
		[Space]
		public float delayBeforeStart;
		public Vector2 intervalRange;
		[Space]
		[HideInInspector] public float timeLeftBeforeSpawn;

		public bool IsReadyToSpawn() { return (timeSinceStart >= delayBeforeStart) && (timeLeftBeforeSpawn <= 0.0f); }
		public void UpdateTimeBeforeSpawn() { timeLeftBeforeSpawn -= Time.deltaTime; }
		public void ResetTimeBeforeSpawn() { timeLeftBeforeSpawn = Random.Range(intervalRange.x, intervalRange.y); }
	}
	#endregion

	[Header("COLLECTIONS")]
	[SerializeField] private List<EnemySpawnType> _spawnTypes;
	public List<Enemy> Enemies { get; private set; }

	[Header("SPAWN DATA")]
	public float spawnRadius = 80f;
	public float spawnPlayerDetectionRadius = 3f;

	private static float timeSinceStart = 0.0f;
	private Transform _rootTransform;


	private void Awake()
	{
		Instance = this;

		if (Enemies == null) { Enemies = new List<Enemy>(); }

		if (_rootTransform == null) { _rootTransform = transform; }
		if (timeSinceStart != 0.0f) { timeSinceStart = 0.0f; }

		InitSpawnTypes();
	}

	private void InitSpawnTypes()
	{
		foreach (EnemySpawnType type in _spawnTypes)
			type.ResetTimeBeforeSpawn();
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
			if (GameManager.IsPlaying)
			{
				timeSinceStart += Time.deltaTime;

				foreach (EnemySpawnType type in _spawnTypes)
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

	public void IncreaseDifficulty(float spawnMultiplier, float enemyStatMultiplier)
	{
		// [TO DO] Increase enemies stats and other behaviours
	}
}

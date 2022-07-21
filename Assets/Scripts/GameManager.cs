using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region SINGLETON
	public static GameManager Instance { get; private set; }
	#endregion

	[Header("GAME SETUP")]
	public int speedOnStart = 3;
	public int livesAmountOnStart = 3;
	public int damageOnStart = 3;
	public int fireRateOnStart = 3;
	public int projectileAmountOnStart = 1;
    public int seekingProjectileAmountOnStart = 0;

	[Header("SCORING")]
	public int score;

	[Header("DIFFICULTY SCALING")]
	public float timeBeforeBuff = 30f;
	[Space]
	public float spawnMultplier = 0.9f;
	public float enemyStatsMultiplier = 1.1f;

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		IncreaseDifficulty();
	}
	#endregion

	#region GAMEPLAY
	private void IncreaseDifficulty() { StartCoroutine(IncreaseDifficulty_Coroutine()); }
	private IEnumerator IncreaseDifficulty_Coroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(timeBeforeBuff);
			// EnemySpawner.Instance.IncreaseDifficulty(spawnMultplier, enemyStatsMultiplier);
		}
	}
	#endregion

	public void GameOver() { StartCoroutine(GameOver_Coroutine()); }
	private IEnumerator GameOver_Coroutine()
	{
		yield return new WaitForSeconds(2f);

		Time.timeScale = 0.05f;
		HUDManager.Instance.ShowGameOverScreen();
	}


	public void OnTryAgainGame()
	{
		SceneManager.LoadScene(0, LoadSceneMode.Single);
		Time.timeScale = 1f;
	}

	public void OnExitGame()
	{
		Application.Quit();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region GAME STATE
public enum GameState
{
	PLAY,   // Default runtime behaviour
	PAUSE,  // Paused but Time.timeScale = 1
	FREEZE  // Paused but Time.timeScale = 0
}
#endregion

public class GameManager : MonoBehaviour
{
	#region SINGLETON
	public static GameManager Instance { get; private set; }
	#endregion

	[Header("GAME STATE")]
	public static GameState gameState = GameState.PLAY;
	public static bool IsPlaying { get { return gameState == GameState.PLAY; } }
	public static bool IsPaused { get { return gameState == GameState.PAUSE; } }
	public static bool IsFrozen { get { return gameState == GameState.FREEZE; } }
	public static bool IsPausedOrFrozen { get { return gameState == (GameState.PAUSE | GameState.FREEZE); } }

	[Header("GAME SETUP")]
	public int speedOnStart = 3;
	public int livesAmountOnStart = 3;
	public int damageOnStart = 3;
	public int fireRateStandardOnStart = 3;
	public int fireRateSeekerOnStart = 0;
	public int projectileAmountOnStart = 1;

	[Header("SPAWNING")]

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

	#region GAME STATE MANAGEMENT
	public static void SwitchGameState(GameState newState)
	{
		switch (newState)
		{
			case GameState.PLAY:
				gameState = newState;
				break;

			case GameState.PAUSE:
				gameState = newState;
				if (Time.timeScale < 1.0f)
					Time.timeScale = 1.0f;
				break;

			case GameState.FREEZE:
				gameState = newState;
				if (Time.timeScale > 0.0f)
					Time.timeScale = 0.0f;
				break;
		}
	}
	#endregion

	#region DIFFICULTY SCALING
	private void IncreaseDifficulty() { StartCoroutine(IncreaseDifficulty_Coroutine()); }
	private IEnumerator IncreaseDifficulty_Coroutine()
	{
		while (true)
		{
			if (IsPlaying)
			{
				yield return new WaitForSeconds(timeBeforeBuff);
				// Difficulty Scaling code here...
			}
			else yield return null;
		}
	}
	#endregion

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

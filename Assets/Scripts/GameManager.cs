using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region GAME STATE
public enum GameState
{
	MAIN_MENU,	// Set when the player is on the Main Menu
	PLAY,		// Default runtime behaviour
	PAUSE,		// Paused state (pause menu should be open)
	GAME_OVER	// Set during the Game Over state
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
	public static bool IsGameOver { get { return gameState == GameState.GAME_OVER; } }

	public static float timeSinceStart { get; private set; }

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;

		Cursor.lockState = CursorLockMode.Confined;

		if (timeSinceStart != 0.0f) { timeSinceStart = 0.0f; }
	}
	#endregion

	private void Update()
	{
		if (IsPlaying && !IsGameOver)
		{
			timeSinceStart += Time.deltaTime;
		}
	}

	#region GAMEPLAY

	#region TIMER
	public static string GetTimerString()
	{
		TimeSpan timespan = new TimeSpan(0, 0, Mathf.FloorToInt(timeSinceStart));

		// Seconds
		string sec = timespan.Seconds >= 10 ? timespan.Seconds.ToString() : $"0{ timespan.Seconds }";

		// Minutes
		string min = timespan.Minutes >= 10 ? timespan.Minutes.ToString() : $"0{ timespan.Minutes }";

		// Hours
		string hou = timespan.Hours >= 10 ? timespan.Hours.ToString() : $"0{ timespan.Hours }";

		if (timespan.Hours > 0)
			return $"{ hou }:{ min }:{ sec }";
		if (timespan.Minutes > 0)
			return $"{ min }:{ sec }";

		return sec;
	}
	#endregion

	#region DICE ROLLS

	#endregion

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
				if (Time.timeScale != 1.0f)
					Time.timeScale = 1.0f;
				break;

			case GameState.GAME_OVER:
				gameState = newState;
				if (Time.timeScale != 0.0f)
					Time.timeScale = 0.0f;
				break;
		}
	}
	#endregion

	#region GAME OVER
	public void GameOver() { StartCoroutine(GameOver_Coroutine()); }
	private IEnumerator GameOver_Coroutine()
	{
		SwitchGameState(GameState.GAME_OVER);

		yield return new WaitForSeconds(2.0f);

		HUDManager.Instance.ShowGameOverScreen();
	}
	#endregion

	#endregion



	public void OnTryAgainGame()
	{
		gameState = GameState.PLAY;

		SceneManager.LoadScene(0, LoadSceneMode.Single);
		Time.timeScale = 1f;
	}

	public void OnExitGame()
	{
		Application.Quit();
	}
}

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
	public int livesAmountOnStart = 3;
	public int damageOnStart = 3;
	public int fireRateOnStart = 3;

	private void Awake()
	{
		Instance = this;
	}

	public void GameOver()
	{
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
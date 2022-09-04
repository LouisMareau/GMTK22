using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class HUDManager : MonoBehaviour
{
	#region SINGLETON
	public static HUDManager Instance { get; private set; }
	#endregion

	private HealthManager healthManager;

	[Header("OVERLAY")]
	[SerializeField] private HUDOverlayData _overlayData;
	public static HUDOverlayData overlayData;

	[Header("SCREENS")]
	[SerializeField] private GameObject _dataOverlayRoot;
	[Space]
	[SerializeField] private GameObject _pauseScreen;
	[Space]
	[SerializeField] private GameObject _gameOverScreen;
	[SerializeField] private TextMeshProUGUI _scoreObtainedLabel;
	[SerializeField] private TextMeshProUGUI _timeAchievedLabel;

	[Header("PLAYER")]
	[SerializeField] private PlayerData _playerData;

	[Header("SCORING")]
	[SerializeField] private TextMeshProUGUI _scoreLabel;

	private void Awake()
	{
		Instance = this;

		overlayData = _overlayData;
	}

	private void Start()
	{
		HideGameOverScreen();
		_dataOverlayRoot.SetActive(false);
		_pauseScreen.SetActive(false);
		_gameOverScreen.SetActive(false);
	}

	private void Update()
	{
		// PAUSE MENU
		if (Input.GetKeyDown(KeyCode.Escape)) { TogglePauseScreen(); }

		// DATA OVERLAY
		if (Input.GetKeyDown(KeyCode.Tab)) { ToggleDataOverlay(); }
		if (IsDataOverlayVisible)
		{
			overlayData.timer.text = GameManager.GetTimerString();
		}
	}

	#region HUD (Default / Data)
	public void UpdateScoreLabel(int score)
	{
		_scoreLabel.text = score.ToString();
	}

	#region DATA OVERLAY
	private void ToggleDataOverlay()
	{
		if (IsPlaying)
		{
			_dataOverlayRoot.SetActive(!_dataOverlayRoot.activeInHierarchy);
		}
	}
	public bool IsDataOverlayVisible { get { return _dataOverlayRoot.activeInHierarchy; } }

	// PLAYER
	public static void UpdatePlayerSpeedLabel(float speed)
	{
		overlayData.playerSpeed.Set("Speed", speed);
	}
	public static void UpdatePlayerDamageLabel(float damage)
	{
		overlayData.playerDamage.Set("Damage", damage);
	}
	public static void UpdatePlayerFireRateStandardLabel(int firerate)
	{
		overlayData.playerFirerateStandard.Set("Firerate (Standard)", firerate);
	}
	public static void UpdatePlayerFireRateSeekerLabel(int firerate)
	{
		overlayData.playerFirerateSeeker.Set("Firerate (Seeker)", firerate);
	}

	// ENEMIES
	public static void UpdateEnemyRandomSpawnInterval(EnemyType type, Vector2 interval)
	{
		switch (type)
		{
			case EnemyType.MELEE_DETONATOR:
				overlayData.enemyMDRandomSpawnInterval.Set("Random Interval Range", interval.x, interval.y);
				break;

			case EnemyType.DIE_HOLDER:
				overlayData.enemyDHRandomSpawnInterval.Set("Random Interval Range", interval.x, interval.y);
				break;

			case EnemyType.PULSAR:
				overlayData.enemyPRandomSpawnInterval.Set("Random Interval Range", interval.x, interval.y);
				break;
		}
	}
	#endregion
	#endregion

	#region GAME OVER
	public void ShowGameOverScreen()
	{
		_scoreObtainedLabel.text = GameRecords.score.ToString();
		_timeAchievedLabel.text = GameManager.GetTimerString();

		_gameOverScreen.SetActive(true);
	}
	public void HideGameOverScreen() { _gameOverScreen.SetActive(false); }
	#endregion

	#region PAUSE / RESUME
	private void TogglePauseScreen()
	{
		if (IsPlaying || IsPaused)
		{
			_pauseScreen.SetActive(!_pauseScreen.activeInHierarchy);

			// We change the game state
			SwitchGameState(IsPlaying ? GameState.PAUSE : GameState.PLAY);
		}
	}

	public void OnResumeGame()
	{
		if (IsPaused)
		{
			_pauseScreen.SetActive(false);

			// We change the game state
			GameManager.SwitchGameState(GameState.PLAY);
		}
	}
	#endregion
}
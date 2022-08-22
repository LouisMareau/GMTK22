using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
		if (IsDataOverlayVisible())
		{
			overlayData.timer.text = GameManager.GetTimerString();
		}
	}

	public void UpdateSpeedLabel(float speed)
	{
		overlayData.playerSpeed.Set("Speed", speed);
	}

	public void UpdateDamageLabel(float damage)
	{
		overlayData.playerDamage.Set("Damage", damage);
	}

	public void UpdateFireRateStandardLabel(float firerate)
	{
		overlayData.playerFirerateStandard.Set("Firerate (Standard)", firerate);
	}

	public void UpdateFireRateSeekerLabel(float firerate)
	{
		overlayData.playerFirerateSeeker.Set("Firerate (Seeker)", firerate);
	}

	public void UpdateScoreLabel(int score)
	{
		_scoreLabel.text = score.ToString();
	}

	public void ShowGameOverScreen()
	{
		_scoreObtainedLabel.text = GameRecords.score.ToString();

		_gameOverScreen.SetActive(true);
	}
	public void HideGameOverScreen() { _gameOverScreen.SetActive(false); }

	private void TogglePauseScreen()
	{
		_pauseScreen.SetActive(!_pauseScreen.activeSelf);

		if (_pauseScreen.activeSelf)
			Time.timeScale = 0.1f;
		else
			Time.timeScale = 1f;
	}

	private void ToggleDataOverlay() { _dataOverlayRoot.SetActive(!_dataOverlayRoot.activeInHierarchy); }
	public bool IsDataOverlayVisible() { return _dataOverlayRoot.activeInHierarchy; }

	public void OnResumeGame()
	{
		_pauseScreen.SetActive(false);
		Time.timeScale = 1f;
	}
}
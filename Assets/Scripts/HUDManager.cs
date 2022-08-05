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

	[Header("LIVES")]
	[SerializeField] private int _livesLeft;
	[SerializeField] private List<GameObject> lives;
	[SerializeField] private Transform _livesContainer;
	[SerializeField] private GameObject _lifePrefab;
	[Space]
	[SerializeField] private Color32 _lifeON;
	[SerializeField] private Color32 _lifeOFF;

	[Header("SHOOTING")]
	[SerializeField] private TextMeshProUGUI _speedLabel;
	[SerializeField] private TextMeshProUGUI _damageLabel;
	[SerializeField] private TextMeshProUGUI _fireRateStandardLabel;
	[SerializeField] private TextMeshProUGUI _fireRateSeekerLabel;
	[SerializeField] private TextMeshProUGUI _projectileAmountLabel;

	[Header("PAUSE (SCREEN)")]
	[SerializeField] private GameObject _pauseScreen;

	[Header("GAME OVER (SCREEN)")]
	[SerializeField] private GameObject _gameOverScreen;
	[SerializeField] private TextMeshProUGUI _scoreObtainedLabel;

	[Header("PLAYER")]
	[SerializeField] private PlayerData _playerData;

	[Header("SCORING")]
	[SerializeField] private TextMeshProUGUI _scoreLabel;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		HideGameOverScreen();
		_pauseScreen.SetActive(false);

		_livesLeft = GameManager.Instance.livesAmountOnStart;
		for (int i = 0; i < _livesLeft; i++)
			AddExtraLife();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			TogglePauseScreen();
	}

	public void AddExtraLife()
	{
		GameObject instance = Instantiate<GameObject>(_lifePrefab, _livesContainer);
		lives.Add(instance);
	}

	public void GainLife(int addedAmount) { StartCoroutine(GainLife_Coroutine(addedAmount)); }
	private IEnumerator GainLife_Coroutine(int addedAmount)
	{
		for (int i = 0; i < addedAmount; i++)
		{
			if (_livesLeft < lives.Count)
				LerpLifeColorOffToOn(lives[_livesLeft].GetComponent<Image>(), 0.3f);
			else
				AddExtraLife();

			_livesLeft++;
			yield return new WaitForSeconds(0.3f);
		}
	}

	public void LoseLife(int removedAmount) { StartCoroutine(LoseLife_Coroutine(removedAmount)); }
	private IEnumerator LoseLife_Coroutine(int removedAmount)
	{
		for (int i = 0; i < removedAmount; i++)
		{
			if (_livesLeft > 0)
				LerpLifeColorOnToOff(lives[_livesLeft - 1].GetComponent<Image>(), 0.3f);

			_livesLeft--;
			yield return new WaitForSeconds(0.3f);
		}
	}

	public void UpdateSpeedLabel(float speed)
	{
		_speedLabel.text = $"Speed: <b>{ speed }</b>";
	}

	public void UpdateDamageLabel(float damage)
	{
		_damageLabel.text = $"Damage: <b>{ damage }</b>";
	}

	public void UpdateFireRateStandardLabel(float fireRate)
	{
		_fireRateStandardLabel.text = $"Fire Rate (Standard): <b>{ fireRate } projectiles/s</b>";
	}

	public void UpdateFireRateSeekerLabel(float fireRate)
	{
		_fireRateSeekerLabel.text = $"Fire Rate (Seeker): <b>{ fireRate } projectiles/s</b>";
	}

	public void UpdateProjectilesPerBurstLabel(int projectileAmount)
	{
		_projectileAmountLabel.text = $"Burst: <b>{ projectileAmount } projectiles/burst</b>";
	}

	public void UpdateScoreLabel(int score)
	{
		_scoreLabel.text = $"SCORE\n<size=12>{ score }</size>";
	}

	public void ShowGameOverScreen()
	{
		_scoreObtainedLabel.text = GameRecords.score.ToString();

		_gameOverScreen.SetActive(true);
	}
	public void HideGameOverScreen() { _gameOverScreen.SetActive(false); }

	private void LerpLifeColorOffToOn(Image image, float duration) { StartCoroutine(LerpLifeColorOffToOn_Corourtine(image, duration)); }
	private IEnumerator LerpLifeColorOffToOn_Corourtine(Image image, float duration)
	{
		float t = 0;
		while (t < duration)
		{
			image.color = Color32.Lerp(_lifeOFF, _lifeON, t / duration);

			t += Time.deltaTime;
			yield return null;
		}
	}

	private void LerpLifeColorOnToOff(Image image, float duration) { StartCoroutine(LerpLifeColorOnToOff_Corourtine(image, duration)); }
	private IEnumerator LerpLifeColorOnToOff_Corourtine(Image image, float duration)
	{
		float t = 0;
		while (t < duration)
		{
			image.color = Color32.Lerp(_lifeON, _lifeOFF, t / duration);

			t += Time.deltaTime;
			yield return null;
		}
	}

	private void TogglePauseScreen()
	{
		_pauseScreen.SetActive(!_pauseScreen.activeSelf);

		if (_pauseScreen.activeSelf)
			Time.timeScale = 0.1f;
		else
			Time.timeScale = 1f;
	}

	public void OnResumeGame()
	{
		_pauseScreen.SetActive(false);
		Time.timeScale = 1f;
	}
}
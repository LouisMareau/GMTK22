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
	[SerializeField] private List<GameObject> lives;
	[SerializeField] private Transform _livesContainer;
	[SerializeField] private GameObject _lifePrefab;
	[Space]
	[SerializeField] private Color _lifeON;
	[SerializeField] private Color _lifeOFF;

	[Header("SHOOTING")]
	[SerializeField] private TextMeshProUGUI _damageLabel;
	[SerializeField] private TextMeshProUGUI _fireRateLabel;

	[Header("GAME OVER")]
	[SerializeField] private GameObject _gameOverScreen;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		HideGameOverScreen();

		for (int i = 0; i < GameManager.Instance.livesAmountOnStart; i++)
			GainLife();
	}

	public void GainLife()
	{
		GameObject instance = Instantiate<GameObject>(_lifePrefab, _livesContainer);
		lives.Add(instance);
	}

	public void GainLife(int currentLives)
	{
		// If the lives are simply less than the starting amount (meaning that the player lost at least a life), we set the life color to ON
		// Otherwiwe, the instantiate a new life
		if (currentLives <= lives.Count)
		{
			lives[currentLives - 1].GetComponent<Image>().color = _lifeON;
		}
		else
		{
			GainLife();
		}
	}

	public void LoseLife(int currentLives)
	{
		if (currentLives >= 0)
		{
			// If we have 2 lives left, we need to discolor the 3rd life (or index 2)
			lives[currentLives].GetComponent<Image>().color = _lifeOFF;
		}
	}

	public void UpdateDamageLabel(int damage)
	{
		_damageLabel.text = $"Damage: { damage }";
	}

	public void UpdateFireRateLabel(int fireRate)
	{
		_fireRateLabel.text = $"Rate of Fire: { fireRate } projectiles /s";
	}

	public void ShowGameOverScreen() { _gameOverScreen.SetActive(true); }
	public void HideGameOverScreen() { _gameOverScreen.SetActive(false); }
}
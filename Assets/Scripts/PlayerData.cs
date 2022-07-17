using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	public int lives;
	public int damage;
	public int fireRate;

	private void Awake()
	{
		lives = GameManager.Instance.livesAmountOnStart;
		damage = GameManager.Instance.damageOnStart;
		fireRate = GameManager.Instance.fireRateOnStart;
	}

	public void GainLife(int amount)
	{
		lives += amount;
		HUDManager.Instance.GainLife(lives);
	}

	public void LoseLife(int amount)
	{
		lives -= amount;
		HUDManager.Instance.LoseLife(lives);

		// Game over when lives <= 0
		if (lives <= 0)
			GameManager.Instance.GameOver();
	}

	public void UpdateDamage(int amount)
	{
		damage += amount;
		HUDManager.Instance.UpdateDamageLabel(damage);
	}

	public void UpdateFireRate(int amount)
	{
		fireRate += amount;
		HUDManager.Instance.UpdateFireRateLabel(fireRate);
	}
}
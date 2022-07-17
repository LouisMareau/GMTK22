using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	public float speed;
	public int lives;
	public float damage;
	public int fireRate;
    public int projectileAmount;
    public int jumpAmount;

	private void Awake()
	{
		speed = GameManager.Instance.speedOnStart;
		lives = GameManager.Instance.livesAmountOnStart;
		damage = GameManager.Instance.damageOnStart;
		fireRate = GameManager.Instance.fireRateOnStart;
        projectileAmount = GameManager.Instance.projectileAmountOnStart;
        jumpAmount = GameManager.Instance.jumpAmountOnStart;
	}

	public void GainLife(int amount)
	{
		lives += amount;
		HUDManager.Instance.GainLife(amount);
	}
	public void LoseLife(int amount)
	{
		lives -= amount;
		HUDManager.Instance.LoseLife(amount);

		// Game over when lives <= 0
		if (lives <= 0)
			GameManager.Instance.GameOver();
	}

	public void UpdateSpeed(float extraAmount)
	{
		speed += extraAmount;
		HUDManager.Instance.UpdateSpeedLabel(speed);
	}
	public void UpdateDamage(float extraAmount)
	{
		damage += extraAmount;
		HUDManager.Instance.UpdateDamageLabel(damage);
	}
	public void UpdateFireRate(int extraAmount)
	{
		fireRate += extraAmount;
		HUDManager.Instance.UpdateFireRateLabel(fireRate);
	}

    public void AddProjectile(int amount) {
        projectileAmount += amount;
		HUDManager.Instance.UpdateProjectilesPerBurstLabel(amount);
	}

    public void AddJump(int amount) {
        jumpAmount += amount;
    }
}

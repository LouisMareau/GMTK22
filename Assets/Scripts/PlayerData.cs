using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
	#region ENUMS
	public enum PlayerStatus
	{
		IDLE,
		CAN_SHOOT,
		CANNOT_SHOOT,
		READY_TO_BE_KILLED
	}
	#endregion

	[SerializeField] private PlayerController _playerController;
	[Space]
	public PlayerStatus status = PlayerStatus.IDLE;
	[Space]
	public float speed;
	public int lives;
	public float damage;
	public int fireRate;
    public int projectileAmount;
    public int projectileSpeedBonus;
    public int projectileRadiusBonus;
    public int projectileKnockback;

	public delegate void OnDamageApplied();
	public static event OnDamageApplied onDamageApplied;

	private void Awake()
	{
		if (_playerController == null) { _playerController = GetComponent<PlayerController>(); }

		speed = GameManager.Instance.speedOnStart;
		lives = GameManager.Instance.livesAmountOnStart;
		damage = GameManager.Instance.damageOnStart;
		fireRate = GameManager.Instance.fireRateOnStart;
        projectileAmount = GameManager.Instance.projectileAmountOnStart;

		status = PlayerStatus.CAN_SHOOT;
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

		if (onDamageApplied != null)
			onDamageApplied.Invoke();

		// Game over when lives <= 0
		if (lives <= 0)
		{
			_playerController.Kill();
			GameManager.Instance.GameOver();
		}
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

    public void AddProjectileSpeed(int amount) {
        projectileSpeedBonus += amount;
    }

    public void AddProjectileRadius(int amount) {
        projectileRadiusBonus += amount;
    }

    public void AddKnockback(int amount) {
        projectileKnockback += amount;
    }
}

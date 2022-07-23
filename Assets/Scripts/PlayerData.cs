using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
	#region ENUMS
	public enum PlayerStatus
	{
		DEFAULT, // Default behaviour during gameplay
		IDLE, // For UI purposes
		CANNOT_SHOOT, // For event purposes (i.e "Time to make peace!")
		NOT_TODAY_ACTIVE // For event purposes (i.e "Not Today!")
	}
	#endregion

	[SerializeField] private PlayerController _playerController;
	[Space]
	public PlayerStatus status = PlayerStatus.DEFAULT;
	[Space]
	public int lives;

	public float speed;

	public float baseDamage;
	public float finalDamage;

	public float baseFireRate;
	public float finalFireRate;

	public int projectileSpreadAmount;

    public int projectileSpeedBonus;

    public float projectileRadiusBonus;

    public int projectileKnockback;

    public int seekingProjectileAmount;

	public delegate void OnFatalDamageApplied();
	public static event OnFatalDamageApplied onFatalDamageApplied;

	#region INITIALIZATION
	private void Awake()
	{
		if (_playerController == null) { _playerController = GetComponent<PlayerController>(); }

		speed = GameManager.Instance.speedOnStart;
		lives = GameManager.Instance.livesAmountOnStart;
		baseDamage = GameManager.Instance.damageOnStart;
		finalDamage = baseDamage;
		baseFireRate = GameManager.Instance.fireRateOnStart;
		finalFireRate = baseFireRate;
		projectileSpreadAmount = GameManager.Instance.projectileAmountOnStart;
        seekingProjectileAmount = GameManager.Instance.seekingProjectileAmountOnStart;

		HUDManager.Instance.UpdateSpeedLabel(speed);
		HUDManager.Instance.UpdateDamageLabel(finalDamage);
		HUDManager.Instance.UpdateFireRateLabel(finalFireRate);

		status = PlayerStatus.DEFAULT;
	}
	#endregion

	public void GainLife(int amount)
	{
		lives += amount;
		HUDManager.Instance.GainLife(amount);
	}
	public void LoseLife(int amount)
	{
		lives -= amount;
		HUDManager.Instance.LoseLife(amount);

		if (lives <= 0)
		{
			if (onFatalDamageApplied != null)
			{
				onFatalDamageApplied.Invoke(); // This will change status to NOT_TODAY_ACTIVE (and start the coroutine associated with the effect "Not Today!")
				GainLife(amount);
			}

			if (status != PlayerStatus.NOT_TODAY_ACTIVE)
			{
				_playerController.Kill();
				GameManager.Instance.GameOver();
			}
		}
	}

	public void UpdateSpeed(float extraAmount)
	{
		speed += extraAmount;
		HUDManager.Instance.UpdateSpeedLabel(speed);
	}

	public void UpdateBaseDamage(float extraAmount)
	{
		baseDamage += extraAmount;
	}
	public void UpdateFinalDamage(float multiplier)
	{
		finalDamage = baseDamage * multiplier;
		HUDManager.Instance.UpdateDamageLabel(finalDamage);
	}

	public void UpdateBaseFireRate(float extraAmount)
	{
		baseFireRate += extraAmount;
	}
	public void UpdateFinalFireRate(float multiplier)
	{
		finalFireRate = baseFireRate * multiplier;
		HUDManager.Instance.UpdateFireRateLabel(finalFireRate);
	}

    public void AddProjectile(int amount) {
        projectileSpreadAmount += amount;
		HUDManager.Instance.UpdateProjectilesPerBurstLabel(amount);
	}

    public void MakeSeekingProjectile(int amount) {
        this.AddProjectile(-1);
        seekingProjectileAmount += amount;
        //TODO Hud
    }

    public void AddProjectileSpeed(int amount) {
        projectileSpeedBonus += amount;
    }

    public void AddProjectileRadius(float amount) {
        projectileRadiusBonus += amount;
    }

    public void AddKnockback(int amount) {
        projectileKnockback += amount;
    }
}

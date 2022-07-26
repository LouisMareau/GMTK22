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
	
	[Header("HEALTH")]
	public int lives;

	[Header("MOVEMENT")]
	public float speed;

	[Header("SHOOTING")]
	public float baseDamage;
	public float finalDamage;

	public float baseFireRateStandard;
	public float finalFireRateStandard;

	public float baseFireRateSeeker;
	public float finalFireRateSeeker;

	public float baseRadius;
	public float finalRadius;


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
		baseFireRateStandard = GameManager.Instance.fireRateStandardOnStart;
		finalFireRateStandard = baseFireRateStandard;
		baseFireRateSeeker = GameManager.Instance.fireRateSeekerOnStart;
		finalFireRateSeeker = baseFireRateSeeker;

		HUDManager.Instance.UpdateSpeedLabel(speed);
		HUDManager.Instance.UpdateDamageLabel(finalDamage);
		HUDManager.Instance.UpdateFireRateStandardLabel(finalFireRateStandard);
		HUDManager.Instance.UpdateFireRateSeekerLabel(finalFireRateSeeker);

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

	public void UpdateBaseFireRateStandard(float extraAmount)
	{
		baseFireRateStandard += extraAmount;
	}
	public void UpdateFinalFireRateStandard(float multiplier)
	{
		finalFireRateStandard = baseFireRateStandard * multiplier;
		HUDManager.Instance.UpdateFireRateStandardLabel(finalFireRateStandard);
	}

	public void UpdateBaseFireRateSeeker(float extraAmount)
	{
		baseFireRateSeeker += extraAmount;
	}
	public void UpdateFinalFireRateSeeker(float multiplier)
	{
		finalFireRateSeeker = baseFireRateSeeker * multiplier;
		HUDManager.Instance.UpdateFireRateSeekerLabel(finalFireRateSeeker);
	}

	public void TransformProjectileStandardIntoSeeker(int amount)
	{
		UpdateBaseFireRateStandard(-1);
		UpdateBaseFireRateSeeker(1);
		HUDManager.Instance.UpdateFireRateStandardLabel(finalFireRateStandard);
		HUDManager.Instance.UpdateFireRateSeekerLabel(finalFireRateStandard);
	}

	public void AddProjectileSpeed(int amount)
	{
		// [TO DO] ...
	}
}

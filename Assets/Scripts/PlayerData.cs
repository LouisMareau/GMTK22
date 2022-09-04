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
	public HealthManager healthManager;

	[Header("MOVEMENT")]
	[HideInInspector] public float speed;

	[Header("SHOOTING")]
	[HideInInspector] public float baseDamage;
	[HideInInspector] public float finalDamage;
	[Space]
	[HideInInspector] public int baseFireRateStandard;
	[HideInInspector] public int finalFireRateStandard;
	[Space]
	[HideInInspector] public int baseFireRateSeeker;
	[HideInInspector] public int finalFireRateSeeker;

	[HideInInspector] public float baseRadius;
	[HideInInspector] public float finalRadius;

	#region INITIALIZATION
	private void Awake()
	{
		if (_playerController == null) { _playerController = GetComponent<PlayerController>(); }

		BaseDataManager.BaseData_Player baseData = BaseDataManager.baseDataPlayer;
		speed = baseData.speed;
		baseDamage = baseData.damage;
		baseFireRateStandard = baseData.firerateStandard;
		baseFireRateSeeker = baseData.firerateSeeker;

		finalDamage = baseDamage;
		finalFireRateStandard = baseFireRateStandard;
		finalFireRateSeeker = baseFireRateSeeker;

		HUDManager.UpdatePlayerSpeedLabel(speed);
		HUDManager.UpdatePlayerDamageLabel(finalDamage);
		HUDManager.UpdatePlayerFireRateStandardLabel(finalFireRateStandard);
		HUDManager.UpdatePlayerFireRateSeekerLabel(finalFireRateSeeker);

		status = PlayerStatus.DEFAULT;
	}
	#endregion

	public void GainHealth(int amount) { healthManager.GainHealth(amount); }
	public void LoseHealth(int amount) { healthManager.LoseHealth(amount); }

	public void UpdateSpeed(float extraAmount)
	{
		speed += extraAmount;
		HUDManager.UpdatePlayerSpeedLabel(speed);
	}

	public void UpdateBaseDamage(float extraAmount) { baseDamage += extraAmount; }
	public void UpdateFinalDamage(float multiplier)
	{
		finalDamage = baseDamage * multiplier;
		HUDManager.UpdatePlayerDamageLabel(finalDamage);
	}

	public void UpdateBaseFireRateStandard(int extraAmount) { baseFireRateStandard += extraAmount; }
	public void UpdateFinalFireRateStandard(int multiplier)
	{
		finalFireRateStandard = baseFireRateStandard * multiplier;
		HUDManager.UpdatePlayerFireRateStandardLabel(finalFireRateStandard);
	}

	public void UpdateBaseFireRateSeeker(int extraAmount) { baseFireRateSeeker += extraAmount; }
	public void UpdateFinalFireRateSeeker(int multiplier)
	{
		finalFireRateSeeker = baseFireRateSeeker * multiplier;
		HUDManager.UpdatePlayerFireRateSeekerLabel(finalFireRateSeeker);
	}

	public void TransformProjectileStandardIntoSeeker(int amount)
	{
		UpdateBaseFireRateStandard(-1);
		UpdateBaseFireRateSeeker(1);
		HUDManager.UpdatePlayerFireRateStandardLabel(finalFireRateStandard);
		HUDManager.UpdatePlayerFireRateSeekerLabel(finalFireRateStandard);
	}

	public void AddProjectileSpeed(int amount)
	{
		// [TO DO] ...
	}
}

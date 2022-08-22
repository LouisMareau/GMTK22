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

	[HideInInspector] public float baseFireRateStandard;
	[HideInInspector] public float finalFireRateStandard;

	[HideInInspector] public float baseFireRateSeeker;
	[HideInInspector] public float finalFireRateSeeker;

	[HideInInspector] public float baseRadius;
	[HideInInspector] public float finalRadius;

	#region INITIALIZATION
	private void Awake()
	{
		if (_playerController == null) { _playerController = GetComponent<PlayerController>(); }

		BaseDataManager.BaseData_Player baseData = BaseDataManager.baseDataPlayer;

		speed = baseData.speed;
		baseDamage = baseData.damage;
		finalDamage = baseDamage;
		baseFireRateStandard = baseData.firerateStandard;
		finalFireRateStandard = baseFireRateStandard;
		baseFireRateSeeker = baseData.firerateSeeker;
		finalFireRateSeeker = baseFireRateSeeker;

		HUDManager.Instance.UpdateSpeedLabel(speed);
		HUDManager.Instance.UpdateDamageLabel(finalDamage);
		HUDManager.Instance.UpdateFireRateStandardLabel(finalFireRateStandard);
		HUDManager.Instance.UpdateFireRateSeekerLabel(finalFireRateSeeker);

		status = PlayerStatus.DEFAULT;
	}
	#endregion

	public void GainHealth(int amount) { healthManager.GainHealth(amount); }
	public void LoseHealth(int amount) { healthManager.LoseHealth(amount); }

	public void UpdateSpeed(float extraAmount)
	{
		speed += extraAmount;
		HUDManager.Instance.UpdateSpeedLabel(speed);
	}

	public void UpdateBaseDamage(float extraAmount) { baseDamage += extraAmount; }
	public void UpdateFinalDamage(float multiplier)
	{
		finalDamage = baseDamage * multiplier;
		HUDManager.Instance.UpdateDamageLabel(finalDamage);
	}

	public void UpdateBaseFireRateStandard(float extraAmount) { baseFireRateStandard += extraAmount; }
	public void UpdateFinalFireRateStandard(float multiplier)
	{
		finalFireRateStandard = baseFireRateStandard * multiplier;
		HUDManager.Instance.UpdateFireRateStandardLabel(finalFireRateStandard);
	}

	public void UpdateBaseFireRateSeeker(float extraAmount) { baseFireRateSeeker += extraAmount; }
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

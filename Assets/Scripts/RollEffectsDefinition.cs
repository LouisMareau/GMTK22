using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEffectsDefinition : MonoBehaviour
{
	#region SINGLETON
	public static RollEffectsDefinition Instance { get; private set; }
	#endregion

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;
	}
	#endregion

	#region ROLL EFFECTS
	public void PowerUp()
	{
		StaticReferences.Instance.playerData.GainLife(1);
	}
	public void PowerUpPlus()
	{
		StaticReferences.Instance.playerData.GainLife(2);
	}
	public void RollWithTheFlow()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		data.UpdateBaseFireRateStandard(1);

		if (!_isMagicFingersExcecuting)
			data.UpdateFinalFireRateStandard(1);
	}
	public void JustNeededSomeGrease()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		data.UpdateBaseFireRateStandard(2);

		if (!_isMagicFingersExcecuting)
			data.UpdateFinalFireRateStandard(1);
	}
	public void SacrebleuItsJammedAgain()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		data.UpdateBaseFireRateStandard(-1);

		if (!_isMagicFingersExcecuting)
			data.UpdateFinalFireRateStandard(1);
	}
	public void IWentToTheShootingRange()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		data.UpdateBaseDamage(0.5f);

		if (!_isEagleEyeExcecuting)
			data.UpdateFinalDamage(1f);
	}
	public void AmericanSniper()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		data.UpdateBaseDamage(1f);

		if (!_isEagleEyeExcecuting)
			data.UpdateFinalDamage(1f);
	}
	public void FearOfDamagingGoods()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		if (data.baseDamage > 0.5f)
		{
			data.UpdateBaseDamage(-0.5f);

			if (!_isEagleEyeExcecuting)
				data.UpdateFinalDamage(1f);
		}
	}
	public void LookAtMyNewGadget()
	{
		PlayerData data = StaticReferences.Instance.playerData;

		// If the fire rate for standard projectile is equal to 0, we don't do anything and we disable the button
		if (data.baseFireRateStandard > 0)
		{
			data.TransformProjectileStandardIntoSeeker(1);

			data.UpdateFinalFireRateStandard(1);
			data.UpdateFinalFireRateSeeker(1);
		}
	}
	
	public void DestabilitatingShots()
	{
		// [TO DO] ... ??
	}
	public void DidIBuyRubberBullets()
	{
		// [TO DO] ... ??
	}
	public void FasterBullets()
	{
		// [TO DO] ... ??
	}
	public void BulletHell() {
		// [TO DO] ... ??
	}
	public void NotYourGrandpasAmmo() {
		// [TO DO] ... ??
	}

	public void KillingFrenzy() { StartCoroutine(KillingFrenzy_Coroutine()); }
	public void JudgementDay() { StartCoroutine(JudgementDay_Coroutine()); }
	public void NotToday() { StartCoroutine(NotToday_Coroutine()); }
	public void MagicFingers() { StartCoroutine(MagicFingers_Coroutine()); }
	public void EagleEye() { StartCoroutine(EagleEye_Coroutine()); }
	public void TimeToMakePeace() { StartCoroutine(TimeToMakePeace_Coroutine()); }
	public void IsThisMagic() { StartCoroutine(IsThisMagic_Coroutine()); }
	#endregion

	#region COROUTINES
	private bool _isKillingFrenzyExcecuting = false;
	private IEnumerator KillingFrenzy_Coroutine()
	{
		_isKillingFrenzyExcecuting = false;
		float duration = 15f;

		int stackableBonuses = 3;
		int stackBonus = 0;

		float timer = 0;
		while (timer < duration)
		{
			if (GameRecords.enemiesKilledSinceLastFrame > 0 &&
				!_isKillingFrenzyExcecuting &&
				stackBonus < stackableBonuses)
			{
				StartCoroutine(KillingFrenzy_CoroutineKillingInterval());
				_isKillingFrenzyExcecuting = true;
				stackBonus++;

				if (stackBonus >= stackableBonuses)
					break;
			}

			timer += Time.deltaTime;
			yield return null;
		}

		StopCoroutine(KillingFrenzy_CoroutineKillingInterval());
	}
	private IEnumerator KillingFrenzy_CoroutineKillingInterval()
	{
		float killingIntervalForBonus = 2f;
		int enemiesKilledForBonus = 7;

		int enemiesKilledWithinInterval = 0;

		float timer = 0;
		while (timer < killingIntervalForBonus)
		{
			enemiesKilledWithinInterval += GameRecords.enemiesKilledSinceLastFrame;

			if (enemiesKilledWithinInterval >= enemiesKilledForBonus)
			{
				StaticReferences.Instance.playerData.GainLife(1);
				break;
			}

			timer += Time.deltaTime;
			yield return null;
		}

		_isKillingFrenzyExcecuting = false;
	}

	private bool _isJudgementDayExcecuting = false;
	private IEnumerator JudgementDay_Coroutine()
	{
		_isJudgementDayExcecuting = false;
		float duration = 15f;

		float timer = 0;
		while (timer < duration)
		{
			if (GameRecords.enemiesKilledSinceLastFrame > 0 &&
				!_isJudgementDayExcecuting)
			{
				StartCoroutine(JudgementDay_CoroutineKillingInterval());
				_isJudgementDayExcecuting = true;
			}

			timer += Time.deltaTime;
			yield return null;
		}

		StopCoroutine(JudgementDay_CoroutineKillingInterval());
	}
	private IEnumerator JudgementDay_CoroutineKillingInterval()
	{
		float killingIntervalForMalus = 1.5f;
		int enemiesKilledForMalus = 3;

		int enemiesKilledWithinInterval = 0;

		float timer = 0;
		while (timer < killingIntervalForMalus)
		{
			enemiesKilledWithinInterval += GameRecords.enemiesKilledSinceLastFrame;

			if (enemiesKilledWithinInterval >= enemiesKilledForMalus)
			{
				StaticReferences.Instance.playerData.LoseLife(1);
				break;
			}

			timer += Time.deltaTime;
			yield return null;
		}

		_isJudgementDayExcecuting = false;
	}

	private bool _hasNotTodayActivated = false;
	private IEnumerator NotToday_Coroutine()
	{
		_hasNotTodayActivated = false;
		float duration = 60f;

		PlayerData.onFatalDamageApplied += NotToday_OnEvent;

		StaticReferences.Instance.playerData.status = PlayerData.PlayerStatus.NOT_TODAY_ACTIVE;

		float timer = 0;
		while (timer < duration)
		{
			if (_hasNotTodayActivated)
				break;

			timer += Time.deltaTime;
			yield return null;
		}

		// At the end of the effect, we simply reset the status to the default one
		StaticReferences.Instance.playerData.status = PlayerData.PlayerStatus.DEFAULT;

		PlayerData.onFatalDamageApplied -= NotToday_OnEvent;
	}

	private bool _isMagicFingersExcecuting = false;
	private IEnumerator MagicFingers_Coroutine()
	{
		float duration = 10f;

		StaticReferences.Instance.playerData.UpdateFinalFireRateStandard(2f);
		_isMagicFingersExcecuting = true;

		float timer = 0;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		_isMagicFingersExcecuting = false;
		StaticReferences.Instance.playerData.UpdateFinalFireRateStandard(1f);
	}

	private bool _isEagleEyeExcecuting = false;
	private IEnumerator EagleEye_Coroutine()
	{
		float duration = 10f;

		StaticReferences.Instance.playerData.UpdateFinalDamage(2f);
		_isEagleEyeExcecuting = true;

		float timer = 0;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		_isEagleEyeExcecuting = false;
		StaticReferences.Instance.playerData.UpdateFinalDamage(1f);
	}

	private IEnumerator TimeToMakePeace_Coroutine()
	{
		float duration = 20f;

		StaticReferences.Instance.playerData.status = PlayerData.PlayerStatus.CANNOT_SHOOT;

		float timer = 0;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		StaticReferences.Instance.playerData.status = PlayerData.PlayerStatus.DEFAULT;
	}

	private IEnumerator IsThisMagic_Coroutine()
	{
		yield return null;
	}
	#endregion

	#region EVENT DECLARATIONS
	private void NotToday_OnEvent()
	{
		_hasNotTodayActivated = true;
	}
	#endregion
}

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

	#region EFFECT-SPECIFIC MEMEBERS
	private bool _notTodayActivated = false;
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
    public void RollWithTheFlow() {
        StaticReferences.Instance.playerData.AddProjectileSpeed(1);
    }
    public void JustNeededSomeGrease() {
        StaticReferences.Instance.playerData.AddProjectileSpeed(2);
    }
    public void SacrebleuItsJammedAgain() {
        StaticReferences.Instance.playerData.AddProjectileSpeed(-1);
    }
    public void IWentToTheShootingRange() {
        StaticReferences.Instance.playerData.UpdateDamage(0.5f);
    }
    public void AmericanSniper() {
        StaticReferences.Instance.playerData.UpdateDamage(1);
    }
    public void FearOfDamagingGoods() {
        StaticReferences.Instance.playerData.UpdateDamage(-0.5f);
    }
    public void LookAtMyNewGadget() {
        StaticReferences.Instance.playerData.AddSeekingProjectile(1);
    }
    public void DestabilitatingShots() {
        StaticReferences.Instance.playerData.AddKnockback(1);
    }
    public void DidIBuyRubberBullets() {
        StaticReferences.Instance.playerData.AddKnockback(-1);
    }
    public void BulletHell() {
        //TODO add bullet spread
    }
    public void NotYourGrandpasAmmo() {
        //TODO maybe you want +10% from base or grom last ?
        StaticReferences.Instance.playerData.AddProjectileRadius(0.1f);
    }

    public void IsThisMagic() { StartCoroutine(IsThisMagic_Coroutine()); }
    public void TimeToMakePeace() { StartCoroutine(TimeToMakePeace_Coroutine()); }
    public void EagleEye() { StartCoroutine(EagleEye_Coroutine()); }
    public void MagicFingers() { StartCoroutine(MagicFingers_Coroutine()); }
	public void KillingFrenzy() { StartCoroutine(KillingFrenzy_Coroutine()); }
	public void JugementDay() { StartCoroutine(JugementDay_Coroutine()); }
	public void NotToday() { StartCoroutine(NotToday_Coroutine()); }
	#endregion

	#region COROUTINES
    private IEnumerator IsThisMagic_Coroutine()
    {
        //TODO
        yield return null;
    }
    private IEnumerator TimeToMakePeace_Coroutine()
    {
        //TODO
        yield return null;
    }
    private IEnumerator EagleEye_Coroutine()
    {
        //TODO
        yield return null;
    }
    private IEnumerator MagicFingers_Coroutine()
    {
        //TODO
        yield return null;
    }
	private IEnumerator KillingFrenzy_Coroutine()
	{
		float duration = 15f;

		float timer = 0;
		while (timer < duration)
		{
			if (GameRecords.enemiesKilledSinceLastFrame > 0)
				StartCoroutine(KillingFrenzy_CoroutineKillingInterval());

			timer += Time.deltaTime;
			yield return null;
		}

		StopCoroutine(KillingFrenzy_CoroutineKillingInterval());
	}
	private IEnumerator KillingFrenzy_CoroutineKillingInterval()
	{
		float killingIntervalForBonus = 2f;
		int enemiesKilledForBonus = 7;
		int stackableBonuses = 3;

		int enemiesKilledWithinInterval = 0;
		int stackBonus = 0;

		float timer = 0;
		while (timer < killingIntervalForBonus)
		{
			enemiesKilledWithinInterval += GameRecords.enemiesKilledSinceLastFrame;

			if (enemiesKilledWithinInterval >= enemiesKilledForBonus &&
				stackBonus < stackableBonuses)
			{
				StaticReferences.Instance.playerData.GainLife(1);
				enemiesKilledWithinInterval = 0;
				stackBonus++;
			}

			if (stackBonus >= stackableBonuses)
				break;

			timer += Time.deltaTime;
			yield return null;
		}
	}
	
	private IEnumerator JugementDay_Coroutine()
	{
		float duration = 15f;

		float timer = 0;
		while (timer < duration)
		{
			if (GameRecords.enemiesKilledSinceLastFrame > 0)
				StartCoroutine(JugementDay_CoroutineKillingInterval());

			timer += Time.deltaTime;
			yield return null;
		}

		StopCoroutine(JugementDay_CoroutineKillingInterval());
	}
	private IEnumerator JugementDay_CoroutineKillingInterval()
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
				enemiesKilledWithinInterval = 0;
			}

			timer += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator NotToday_Coroutine()
	{
		float duration = 60f;

		float timer = 0;
		while (timer < duration)
		{
			if (_notTodayActivated)
				break;

			PlayerData.onDamageApplied += NotToday_EventDeclaration;

			timer += Time.deltaTime;
			yield return null;
		}

		PlayerData.onDamageApplied -= NotToday_EventDeclaration;
	}
	#endregion

	#region EVENT DECLARATIONS
	private void NotToday_EventDeclaration()
	{
		StaticReferences.Instance.playerData.GainLife(1);
		_notTodayActivated = true;
	}
	#endregion
}

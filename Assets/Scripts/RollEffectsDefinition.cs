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
	public void KillingFrenzy() { StartCoroutine(KillingFrenzy_Coroutine()); }
	public void JugementDay() { StartCoroutine(JugementDay_Coroutine()); }
	public void NotToday() { StartCoroutine(NotToday_Coroutine()); }
	#endregion

	#region COROUTINES
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
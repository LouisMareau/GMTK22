using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	#region SINGLETON
	public static HealthManager Instance { get; private set; }
	#endregion

	public static int Health { get; private set; }

	[SerializeField] private HealthBarContainer _healthBarContainer;

	public delegate void OnFatalDamageApplied();
	public static event OnFatalDamageApplied onFatalDamageApplied;

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;

		Health = BaseDataManager.baseDataPlayer.health;
	}
	#endregion

	#region TESTS
	[ContextMenu("Health +1")] public void GainHealthTest_1() { GainHealth(1); }
	[ContextMenu("Health +3")] public void GainHealthTest_3() { GainHealth(3); }
	[ContextMenu("Health +5")] public void GainHealthTest_5() { GainHealth(5); }
	[ContextMenu("Health +10")] public void GainHealthTest_10() { GainHealth(10); }
	[ContextMenu("Health -1")] public void LoseHealthTest_1() { LoseHealth(1); }
	[ContextMenu("Health -3")] public void LoseHealthTest_3() { LoseHealth(3); }
	[ContextMenu("Health -5")] public void LoseHealthTest_5() { LoseHealth(5); }
	[ContextMenu("Health -10")] public void LoseHealthTest_10() { LoseHealth(10); }
	#endregion

	public void GainHealth(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Health++;
			_healthBarContainer.GainHealth();
		}
	}

	public void LoseHealth(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Health--;
			_healthBarContainer.LoseHealth();

			if (Health <= 0)
			{
				if (StaticReferences.Instance.playerData.status != PlayerData.PlayerStatus.NOT_TODAY_ACTIVE)
				{
					StaticReferences.Instance.playerController.Kill();
					GameManager.Instance.GameOver();
				}

				if (onFatalDamageApplied != null)
				{
					onFatalDamageApplied.Invoke(); // This will change status back to DEFAULT (see RollEffectsDefinition.NotToday_Coroutine())
					Health = 1;
					_healthBarContainer.GainHealth();
				}

				break;
			}

		}
	}
}
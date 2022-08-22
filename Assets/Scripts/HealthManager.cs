using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// For any amount below or equal to 10 health and for the first time (a full health bar), we animate each new health bar being added.<br/>
/// Otherwise, we leave empty health bars for unused health and actualize the multiplier for anything 11+ health (on the right side of the full bar).
/// </summary>
/// <remarks>
/// <para> Example: 23 health = <c>[x] [x] [x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] x2</c></para>
/// <para> Example: 17 health = <c>[x] [x] [x] [x] [x] [x] [x] [ ] [ ] [ ] x1</c></para>
/// <para> Example: 4 health (if the bar was completed previously) = <c>[x] [x] [x] [x] [ ] [ ] [ ] [ ] [ ] [ ]</c></para>
/// <para> Example: 4 health (if has never been higher previously) = <c>[x] [x] [x] [x]</c></para>
/// </remarks>

public class HealthManager : MonoBehaviour
{
	#region SINGLETON
	public static HealthManager Instance { get; private set; }
	#endregion

	public int Health { get; private set; }

	private List<HealthBar> _healthBarInstances;

	[SerializeField] private GameObject _healthBarPrefab;
	[SerializeField] private Transform _healthBarInstancesContainer;
	[SerializeField] private TextMeshProUGUI _healthCompletedBarMultiplier;

	public delegate void OnFatalDamageApplied();
	public static event OnFatalDamageApplied onFatalDamageApplied;

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;

		_healthBarInstances = new List<HealthBar>();

		GainHealth(BaseDataManager.baseDataPlayer.health);
	}
	#endregion

	public void GainHealth(int amount) { StartCoroutine(GainHealth_Coroutine(amount)); }
	public void LoseHealth(int amount) { StartCoroutine(LoseHealth_Coroutine(amount)); }

	#region COROUTINES
	private IEnumerator GainHealth_Coroutine(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Health++;

			if (!IsHealthBarCompleted())
			{
				// Health bar multiplier
				if (IsCompletedBarMultiplierVisible())
					SetCompletedBarMultiplierVisibility(false);

				yield return AddNewHealthBar(0.1f);
			}
			else
			{
				// Health bar multiplier
				if (!IsCompletedBarMultiplierVisible())
					SetCompletedBarMultiplierVisibility(true);

				if (Health > 10)
					SetCompletedBarMultiplier();

				// We check for the case when the health gains a new multiplier
				// Example: 10n -> 10n + 1
				// [x] [x] [x] [x] [x] [x] [x] [x] [x] [x]
				//                ↓   ↓   ↓               
				// [x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
				if (Health % 10 == 1)
				{
					yield return EmptyAllHealthBars(0.05f);
					yield return FillHealthBar(_healthBarInstances[0], 0.1f);
				}
				else
					yield return FillHealthBar(GetHealthBarFromCurrentIndex(), 0.2f);
			}

			yield return null;
		}
	}

	public IEnumerator LoseHealth_Coroutine(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			// We check for the case when the health looses a multiplier
			// Example: 10n + 1 -> 10n
			// [x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
			//                ↓   ↓   ↓               
			// [x] [x] [x] [x] [x] [x] [x] [x] [x] [x]
			if ((Health / 10 >= 1) && (Health % 10 == 1))
			{
				yield return EmptyHealthBar(_healthBarInstances[0], 0.1f);
				yield return FillAllHealthBars(0.05f);
			}
			else
				yield return EmptyHealthBar(GetHealthBarFromCurrentIndex(), 0.2f);
			
			Health--;

			// Health bar multiplier
			if (Health <= 10 && IsCompletedBarMultiplierVisible())
				SetCompletedBarMultiplierVisibility(false);

			if (Health > 10)
				SetCompletedBarMultiplier();

			if (Health <= 0)
			{
				if (onFatalDamageApplied != null)
				{
					onFatalDamageApplied.Invoke(); // This will change status to NOT_TODAY_ACTIVE (and start the coroutine associated with the effect "Not Today!")
					GainHealth(amount);
				}

				if (StaticReferences.Instance.playerData.status != PlayerData.PlayerStatus.NOT_TODAY_ACTIVE)
				{
					StaticReferences.Instance.playerController.Kill();
					GameManager.Instance.GameOver();
				}
			}

			yield return null;
		}
	}

	private HealthBar GetHealthBarFromCurrentIndex()
	{
		int reminder = Health % 10;

		if ((Health > 0) && (reminder == 0))
			return _healthBarInstances[9];
		else
			return _healthBarInstances[reminder - 1];
	}

	private IEnumerator AddNewHealthBar(float animDuration)
	{
		HealthBar healthBarInstance = Instantiate<GameObject>(_healthBarPrefab, _healthBarInstancesContainer).GetComponent<HealthBar>();
		_healthBarInstances.Add(healthBarInstance);

		yield return healthBarInstance.Fill(animDuration);
	}

	private IEnumerator FillHealthBar(HealthBar healthBar, float animDuration)
	{
		yield return healthBar.Fill(animDuration);
	}
	private IEnumerator EmptyHealthBar(HealthBar healthBar, float animDuration)
	{
		yield return healthBar.Empty(animDuration);
	}

	private IEnumerator FillAllHealthBars(float animDuration)
	{
		for (int i = 0; i < _healthBarInstances.Count; i++)
		{
			yield return FillHealthBar(_healthBarInstances[i], animDuration);
			yield return null;
		}
	}
	private IEnumerator EmptyAllHealthBars(float animDuration)
	{
		for (int i = _healthBarInstances.Count - 1; i >= 0; i--)
		{
			yield return EmptyHealthBar(_healthBarInstances[i], animDuration);
			yield return null;
		}
	}
	#endregion

	#region QUALITY OF LIFE
	private bool IsHealthBarCompleted() { return _healthBarInstances.Count == 10; }
	private bool IsCompletedBarMultiplierVisible() { return _healthCompletedBarMultiplier.gameObject.activeInHierarchy; }
	private void SetCompletedBarMultiplierVisibility(bool isVisible) { _healthCompletedBarMultiplier.gameObject.SetActive(isVisible); }
	private void SetCompletedBarMultiplier() { _healthCompletedBarMultiplier.text = $"x{ Mathf.FloorToInt(Health / 10) }"; }
	#endregion
}
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For any amount below or equal to 10 health and for the first time completed (total of 10 bar units), each new health bar unit being added should animate.<br/>
/// Otherwise, we leave empty health bars for unused health and actualize the multiplier for anything 11+ health (on the right side of the completed health bar).
/// </summary>
/// <remarks>
/// <para>
/// BONUS:<br/>
/// As the player gains and looses health, each gain/loss animation can be interrupted by another gain/loss of health.<br/>
/// This health bar animation system must be robust enough to enable animation interruptions.
/// </para>
/// <para>
/// Example: 23 health = <c>[x] [x] [x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] x2</c><br/>
/// Example: 17 health = <c>[x] [x] [x] [x] [x] [x] [x] [ ] [ ] [ ] x1</c><br/>
/// Example: 4 health (if the bar was completed previously) = <c>[x] [x] [x] [x] [ ] [ ] [ ] [ ] [ ] [ ]</c><br/>
/// Example: 4 health (if has never been higher previously) = <c>[x] [x] [x] [x]</c>
/// </para>
/// <para>
/// Example for going from 20 to 21 health:<br/>
/// <c>[x] [x] [x] [x] [x] [x] [x] [x] [x] [x] x1</c><br/>
/// <c>[x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] x2</c>
/// </para>
/// <para>
/// Example for going from 31 to 30 health:<br/>
/// <c>[x] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] x3</c><br/>
/// <c>[x] [x] [x] [x] [x] [x] [x] [x] [x] [x] x2</c>
/// </para>
/// </remarks>
public class HealthBarContainer : MonoBehaviour
{

	public List<HealthBar> BarUnits { get; private set; }

	public GameObject barUnitPrefab;

	[Header("LOCAL REFERENCES")]
	public Transform root;

	[Header("SCENE REFERENCES")]
	[SerializeField] private HealthBarMultiplier _healthBarMultiplier;

	public int UnitCount { get { return BarUnits.Count; } }
	public bool IsComplete { get { return BarUnits.Count == 10; } }
	private int HealthIndex
	{
		get
		{
			int reminder = HealthManager.Health % 10;
			if (reminder == 0) // Any multiple of 10
				return 9;
			else
				return reminder - 1;
		}
	}

	#region INITIALIZATION
	private void Awake()
	{
		if (BarUnits == null) { BarUnits = new List<HealthBar>(); }
		if (root == null) { root = transform; }
	}

	private void Start()
	{
		ResetBarUnits();

		for (int i = 0; i < HealthManager.Health; i++)
			AddBarUnit();

		_healthBarMultiplier.SetLabel(HealthManager.Health / 10);
	}
	#endregion

	public void GainHealth()
	{
		if (IsComplete && (HealthIndex == 0))
		{
			EmptyAllBarUnits(true);
			_healthBarMultiplier.SetLabel(HealthManager.Health / 10);
		}
		else
		{
			if (!IsComplete && (HealthIndex > BarUnits.Count - 1))
				AddBarUnit();
			else
				BarUnits[HealthIndex].FillInstant();
		}
	}

	public void LoseHealth()
	{
		if (IsComplete && (HealthIndex == 9))
		{
			FillAllBarUnits();
			_healthBarMultiplier.SetLabel((HealthManager.Health / 10) - 1);
		}
		else if (HealthManager.Health == 0)
			BarUnits[0].EmptyInstant();
		else
			BarUnits[HealthIndex+1].EmptyInstant();
	}

	private void AddBarUnit()
	{
		if (BarUnits.Count < 10)
		{
			HealthBar instance = Instantiate<GameObject>(barUnitPrefab, root).GetComponent<HealthBar>();
			instance.FillInstant();

			BarUnits.Add(instance);
		}
	}

	private void FillAllBarUnits()
	{
		foreach (HealthBar barUnit in BarUnits)
			barUnit.FillInstant();
	}

	private void EmptyAllBarUnits(bool shouldFillFirst = false)
	{
		foreach (HealthBar barUnit in BarUnits)
			barUnit.EmptyInstant();

		if (shouldFillFirst)
			BarUnits[0].FillInstant();
	}

	public void ResetBarUnits()
	{
		if (root.childCount == 0) return;

		for (int i = 0; i < root.childCount; i++)
			Destroy(root.GetChild(i).gameObject);

		BarUnits = new List<HealthBar>();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
	public static RollEffects Effects { get; private set; }

	#region INITIALIZATION
	private void Awake()
	{
		Effects = new RollEffects();
	}
	#endregion
}
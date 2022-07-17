using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticReferences : MonoBehaviour
{
	#region SINGLETON
	public static StaticReferences Instance { get; private set; }
	#endregion

	public Transform enemyContainer;
	public Transform vfxContainer;
	public Transform diceContainer;
	public Transform projectileContainer;

	private void Awake()
	{
		Instance = this;
	}
}
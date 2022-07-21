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
	[Space]
	public Transform playerTransform;
	public PlayerData playerData;

	private void Awake()
	{
		Instance = this;

		if (playerData == null) { playerData = playerTransform.GetComponent<PlayerData>(); }
	}
}
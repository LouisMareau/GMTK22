using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecords : MonoBehaviour
{
	public static int enemyMeleeDetonatorKilled = 0;
	public static int enemyDieHolderKilled = 0;
	public static int enemyPulsarKilled = 0;

	public static int enemiesKilledSinceLastFrame;

	#region INITIALIZATION
	private void Awake()
	{
		enemyMeleeDetonatorKilled = 0;
		enemyDieHolderKilled = 0;
		enemyPulsarKilled = 0;
	}
	#endregion

	private void LateUpdate()
	{
		if (enemiesKilledSinceLastFrame > 0)
			enemiesKilledSinceLastFrame = 0;
	}
}
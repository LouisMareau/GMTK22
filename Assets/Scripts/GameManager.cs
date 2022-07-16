using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region SINGLETON
	public static GameManager Instance { get; private set; }
	#endregion

	[Header("GAME SETUP")]
	public int livesAmountOnStart = 3;
	public int fireRateOnStart = 3;

	private void Awake()
	{
		Instance = this;
	}
}
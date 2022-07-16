using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	[HideInInspector] public int lives;

	private void Awake()
	{
		lives = GameManager.Instance.livesAmountOnStart;
	}
}
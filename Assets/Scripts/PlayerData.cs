using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	[HideInInspector] public int lives;
	public float damage = 1;

	private void Awake()
	{
		lives = GameManager.Instance.livesAmountOnStart;
	}

	[ContextMenu("Life +1")]
	public void GainLife()
	{
		lives++;
		HUDManager.Instance.GainLife(lives);
	}


	[ContextMenu("Life -1")]
	public void LoseLife()
	{
		lives--;
		HUDManager.Instance.LoseLife(lives);
	}
}
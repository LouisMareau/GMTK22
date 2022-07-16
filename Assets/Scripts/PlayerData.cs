using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	[HideInInspector] public int lives;
	[HideInInspector] public float damage;

	private void Awake()
	{
		lives = GameManager.Instance.livesAmountOnStart;
		damage = GameManager.Instance.fireRateOnStart;
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
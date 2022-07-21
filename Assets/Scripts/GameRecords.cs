using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecords : MonoBehaviour
{
	public static int enemiesKilledSinceLastFrame;

	private void LateUpdate()
	{
		if (enemiesKilledSinceLastFrame > 0)
			enemiesKilledSinceLastFrame = 0;
	}
}
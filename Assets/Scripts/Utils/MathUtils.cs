using UnityEngine;

public class MathUtils
{
	public static float GetRadiansFromDegrees(float angleInDegrees)
	{
		return angleInDegrees * Mathf.Deg2Rad;
	}
}
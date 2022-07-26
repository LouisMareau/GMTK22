using UnityEngine;

public class MathUtils
{
	public static float GetRadiansFromDegrees(float angleInDegrees)
	{
		return angleInDegrees * Mathf.Deg2Rad;
	}

	public static Vector3 GetVectorFromAngle(float angle)
	{
		float angleRad = angle * (Mathf.PI / 180);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}
}
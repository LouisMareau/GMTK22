using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature_RotatingBlades : EnemyFeature
{
	[Space]
	[SerializeField] private float _distanceDetectionBeforeBladesMinSpeedActivation = 50f;
	[SerializeField] private float _distanceDetectionBeforeBladesMaxSpeedActivation = 20f;
	[Space]
	[SerializeField] private float _bladesMinAngularSpeed = 30f;
	[SerializeField] private float _bladesMidAngularSpeed = 120f;
	[SerializeField] private float _bladesMaxAngularSpeed = 720f;

	private void Update()
	{
		if (_associatedEnemy.distanceFromPlayer <= _distanceDetectionBeforeBladesMaxSpeedActivation)
			RotateBlades(_bladesMaxAngularSpeed);
		else if (_associatedEnemy.distanceFromPlayer <= _distanceDetectionBeforeBladesMinSpeedActivation)
			RotateBlades(_bladesMidAngularSpeed);
		else
			RotateBlades(_bladesMinAngularSpeed);
	}

	#region GAMEPLAY
	private void RotateBlades(float angularSpeed)
	{
		_rootTransform.Rotate(Vector3.up, angularSpeed * Time.deltaTime);
	}
	#endregion
}
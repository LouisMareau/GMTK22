using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice6 : Dice
{
	private Ray _detectionRaySide1;
	private Ray _detectionRaySide2;
	private Ray _detectionRaySide3;
	private Ray _detectionRaySide4;
	private Ray _detectionRaySide5;
	private Ray _detectionRaySide6;

	#region EDITOR-ONLY
	private void OnValidate()
	{
		CalculateDetectionRays();
	}
	#endregion

	protected override void Update()
	{
		base.Update();

		if (GameManager.IsPlaying)
		{
			if (!rigidbody.isKinematic)
				DetectResult();
		}
	}

	#region DICE RESULT MANAGEMENT
	protected override void DetectResult()
	{
		CalculateDetectionRays();

		if (Physics.Raycast(_detectionRaySide1, LayerMask.GetMask("Floor")))
			Result = 1;
		else if (Physics.Raycast(_detectionRaySide2, LayerMask.GetMask("Floor")))
			Result = 2;
		else if (Physics.Raycast(_detectionRaySide3, LayerMask.GetMask("Floor")))
			Result = 3;
		else if (Physics.Raycast(_detectionRaySide4, LayerMask.GetMask("Floor")))
			Result = 4;
		else if (Physics.Raycast(_detectionRaySide5, LayerMask.GetMask("Floor")))
			Result = 5;
		else if (Physics.Raycast(_detectionRaySide6, LayerMask.GetMask("Floor")))
			Result = 6;
	}

	protected override void CalculateDetectionRays()
	{
		_detectionRaySide1 = CastRayToLocalDirection(Vector3.down, _rayDetectionLength);
		_detectionRaySide2 = CastRayToLocalDirection(Vector3.right, _rayDetectionLength);
		_detectionRaySide3 = CastRayToLocalDirection(Vector3.forward, _rayDetectionLength);
		_detectionRaySide4 = CastRayToLocalDirection(Vector3.back, _rayDetectionLength);
		_detectionRaySide5 = CastRayToLocalDirection(Vector3.left, _rayDetectionLength);
		_detectionRaySide6 = CastRayToLocalDirection(Vector3.up, _rayDetectionLength);
	}
	#endregion

	#region DEBUG
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(_detectionRaySide1);
		Gizmos.DrawRay(_detectionRaySide2);
		Gizmos.DrawRay(_detectionRaySide3);
		Gizmos.DrawRay(_detectionRaySide4);
		Gizmos.DrawRay(_detectionRaySide5);
		Gizmos.DrawRay(_detectionRaySide6);
	}
	#endregion
}

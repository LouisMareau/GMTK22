using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die6 : Die
{
	private Ray _detectionRaySide1;
	private Ray _detectionRaySide2;
	private Ray _detectionRaySide3;
	private Ray _detectionRaySide4;
	private Ray _detectionRaySide5;
	private Ray _detectionRaySide6;

	protected override void Update()
	{
		base.Update();

		if (GameManager.IsPlaying)
		{
			if (!rigidbody.isKinematic)
				DetectResult();
		}
	}

	#region DIE RESULT MANAGEMENT
	private void DetectResult()
	{
		_detectionRaySide1 = CastRayToLocalDirection(Vector3.down, 2f);
		_detectionRaySide2 = CastRayToLocalDirection(Vector3.right, 2f);
		_detectionRaySide3 = CastRayToLocalDirection(Vector3.forward, 2f);
		_detectionRaySide4 = CastRayToLocalDirection(Vector3.back, 2f);
		_detectionRaySide5 = CastRayToLocalDirection(Vector3.left, 2f);
		_detectionRaySide6 = CastRayToLocalDirection(Vector3.up, 2f);

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

	private Ray CastRayToLocalDirection(Vector3 direction, float length)
	{
		return new Ray(_meshTransform.position, (_meshTransform.rotation * direction).normalized * length);
	}
	#endregion
}

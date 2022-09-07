using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice20 : Dice
{
	[SerializeField] private List<Transform> _sidePointsRoots;
	private List<Vector3> _sidePoints;

	private Ray _detectionRaySide1;
	private Ray _detectionRaySide2;
	private Ray _detectionRaySide3;
	private Ray _detectionRaySide4;
	private Ray _detectionRaySide5;
	private Ray _detectionRaySide6;
	private Ray _detectionRaySide7;
	private Ray _detectionRaySide8;
	private Ray _detectionRaySide9;
	private Ray _detectionRaySide10;
	private Ray _detectionRaySide11;
	private Ray _detectionRaySide12;
	private Ray _detectionRaySide13;
	private Ray _detectionRaySide14;
	private Ray _detectionRaySide15;
	private Ray _detectionRaySide16;
	private Ray _detectionRaySide17;
	private Ray _detectionRaySide18;
	private Ray _detectionRaySide19;
	private Ray _detectionRaySide20;

	#region EDITOR-ONLY
	private void OnValidate()
	{
		Init();
	}
	#endregion

	#region INITIALIZATION
	private void Init()
	{
		if (_sidePoints == null) { _sidePoints = new List<Vector3>(); }
		if (_sidePointsRoots != null)
		{
			_sidePoints.Clear();
			foreach (Transform t in _sidePointsRoots)
				_sidePoints.Add(t.position);
		}

		if (_sidePoints.Count == 20)
			CalculateDetectionRays();
	}
	protected override void Awake()
	{
		base.Awake();

		Init();
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
			Result = 20;
		else if (Physics.Raycast(_detectionRaySide2, LayerMask.GetMask("Floor")))
			Result = 19;
		else if (Physics.Raycast(_detectionRaySide3, LayerMask.GetMask("Floor")))
			Result = 18;
		else if (Physics.Raycast(_detectionRaySide4, LayerMask.GetMask("Floor")))
			Result = 17;
		else if (Physics.Raycast(_detectionRaySide5, LayerMask.GetMask("Floor")))
			Result = 16;
		else if (Physics.Raycast(_detectionRaySide6, LayerMask.GetMask("Floor")))
			Result = 15;
		else if (Physics.Raycast(_detectionRaySide7, LayerMask.GetMask("Floor")))
			Result = 14;
		else if (Physics.Raycast(_detectionRaySide8, LayerMask.GetMask("Floor")))
			Result = 13;
		else if (Physics.Raycast(_detectionRaySide9, LayerMask.GetMask("Floor")))
			Result = 12;
		else if (Physics.Raycast(_detectionRaySide10, LayerMask.GetMask("Floor")))
			Result = 11;
		else if (Physics.Raycast(_detectionRaySide11, LayerMask.GetMask("Floor")))
			Result = 10;
		else if (Physics.Raycast(_detectionRaySide12, LayerMask.GetMask("Floor")))
			Result = 9;
		else if (Physics.Raycast(_detectionRaySide13, LayerMask.GetMask("Floor")))
			Result = 8;
		else if (Physics.Raycast(_detectionRaySide14, LayerMask.GetMask("Floor")))
			Result = 7;
		else if (Physics.Raycast(_detectionRaySide15, LayerMask.GetMask("Floor")))
			Result = 6;
		else if (Physics.Raycast(_detectionRaySide16, LayerMask.GetMask("Floor")))
			Result = 5;
		else if (Physics.Raycast(_detectionRaySide17, LayerMask.GetMask("Floor")))
			Result = 4;
		else if (Physics.Raycast(_detectionRaySide18, LayerMask.GetMask("Floor")))
			Result = 3;
		else if (Physics.Raycast(_detectionRaySide19, LayerMask.GetMask("Floor")))
			Result = 2;
		else if (Physics.Raycast(_detectionRaySide20, LayerMask.GetMask("Floor")))
			Result = 1;
	}

	protected override void CalculateDetectionRays()
	{
		_detectionRaySide1 = CastRayToLocalDirection(_sidePoints[0], _rayDetectionLength);
		_detectionRaySide2 = CastRayToLocalDirection(_sidePoints[1], _rayDetectionLength);
		_detectionRaySide3 = CastRayToLocalDirection(_sidePoints[2], _rayDetectionLength);
		_detectionRaySide4 = CastRayToLocalDirection(_sidePoints[3], _rayDetectionLength);
		_detectionRaySide5 = CastRayToLocalDirection(_sidePoints[4], _rayDetectionLength);
		_detectionRaySide6 = CastRayToLocalDirection(_sidePoints[5], _rayDetectionLength);
		_detectionRaySide7 = CastRayToLocalDirection(_sidePoints[6], _rayDetectionLength);
		_detectionRaySide8 = CastRayToLocalDirection(_sidePoints[7], _rayDetectionLength);
		_detectionRaySide9 = CastRayToLocalDirection(_sidePoints[8], _rayDetectionLength);
		_detectionRaySide10 = CastRayToLocalDirection(_sidePoints[9], _rayDetectionLength);
		_detectionRaySide11 = CastRayToLocalDirection(_sidePoints[10], _rayDetectionLength);
		_detectionRaySide12 = CastRayToLocalDirection(_sidePoints[11], _rayDetectionLength);
		_detectionRaySide13 = CastRayToLocalDirection(_sidePoints[12], _rayDetectionLength);
		_detectionRaySide14 = CastRayToLocalDirection(_sidePoints[13], _rayDetectionLength);
		_detectionRaySide15 = CastRayToLocalDirection(_sidePoints[14], _rayDetectionLength);
		_detectionRaySide16 = CastRayToLocalDirection(_sidePoints[15], _rayDetectionLength);
		_detectionRaySide17 = CastRayToLocalDirection(_sidePoints[16], _rayDetectionLength);
		_detectionRaySide18 = CastRayToLocalDirection(_sidePoints[17], _rayDetectionLength);
		_detectionRaySide19 = CastRayToLocalDirection(_sidePoints[18], _rayDetectionLength);
		_detectionRaySide20 = CastRayToLocalDirection(_sidePoints[19], _rayDetectionLength);
	}
	#endregion

	#region DEBUG
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(_detectionRaySide1);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_detectionRaySide2);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(_detectionRaySide3);
		Gizmos.DrawRay(_detectionRaySide4);
		Gizmos.DrawRay(_detectionRaySide5);
		Gizmos.DrawRay(_detectionRaySide6);
		Gizmos.DrawRay(_detectionRaySide7);
		Gizmos.DrawRay(_detectionRaySide8);
		Gizmos.DrawRay(_detectionRaySide9);
		Gizmos.DrawRay(_detectionRaySide10);
		Gizmos.DrawRay(_detectionRaySide11);
		Gizmos.DrawRay(_detectionRaySide12);
		Gizmos.DrawRay(_detectionRaySide13);
		Gizmos.DrawRay(_detectionRaySide14);
		Gizmos.DrawRay(_detectionRaySide15);
		Gizmos.DrawRay(_detectionRaySide16);
		Gizmos.DrawRay(_detectionRaySide17);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(_detectionRaySide18);
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(_detectionRaySide19);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(_detectionRaySide20);
	}
	#endregion
}
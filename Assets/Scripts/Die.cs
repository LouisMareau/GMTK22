using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	public DieMenu dieMenu;
	public new Rigidbody rigidbody;

	private Transform _rootTransform;
	private Transform _meshTransform;

	private Ray rayDetectionSide1;
	private Ray rayDetectionSide2;
	private Ray rayDetectionSide3;
	private Ray rayDetectionSide4;
	private Ray rayDetectionSide5;
	private Ray rayDetectionSide6;

	public int diceResult = -1;

	private void Awake()
	{
		_rootTransform = transform;
		_meshTransform = _rootTransform.Find("Mesh");
	}

	void Start()
	{
		if (dieMenu == null) dieMenu = this.GetComponentInChildren<DieMenu>();
		if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();

		rigidbody.isKinematic = true;
	}

	public void Update()
	{
		rayDetectionSide1 = new Ray(_meshTransform.position, Vector3.down * 3f);
		rayDetectionSide2 = new Ray(_meshTransform.position, Vector3.left * 3f);
		rayDetectionSide3 = new Ray(_meshTransform.position, Vector3.forward * 3f);
		rayDetectionSide4 = new Ray(_meshTransform.position, Vector3.back * 3f);
		rayDetectionSide5 = new Ray(_meshTransform.position, Vector3.right * 3f);
		rayDetectionSide6 = new Ray(_meshTransform.position, Vector3.up * 3f);

		if (Physics.Raycast(rayDetectionSide1, LayerMask.GetMask("Floor")))
			diceResult = 1;
		else if (Physics.Raycast(rayDetectionSide2, LayerMask.GetMask("Floor")))
			diceResult = 2;
		else if (Physics.Raycast(rayDetectionSide3, LayerMask.GetMask("Floor")))
			diceResult = 3;
		else if (Physics.Raycast(rayDetectionSide4, LayerMask.GetMask("Floor")))
			diceResult = 4;
		else if (Physics.Raycast(rayDetectionSide5, LayerMask.GetMask("Floor")))
			diceResult = 5;
		else if (Physics.Raycast(rayDetectionSide6, LayerMask.GetMask("Floor")))
			diceResult = 6;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player") {
			//bring random menu if the die result is set
			dieMenu.Launch();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(rayDetectionSide1);
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(rayDetectionSide2);
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(rayDetectionSide3);
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(rayDetectionSide4);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(rayDetectionSide5);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(rayDetectionSide6);
	}
}

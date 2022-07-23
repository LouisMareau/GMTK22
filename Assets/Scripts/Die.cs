using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	public DieMenu dieMenu;
	public new Rigidbody rigidbody;

	private Transform _rootTransform;
	private Transform _meshTransform;
	private Transform _menuTransform;

	private Ray rayDetectionSide1;
	private Ray rayDetectionSide2;
	private Ray rayDetectionSide3;
	private Ray rayDetectionSide4;
	private Ray rayDetectionSide5;
	private Ray rayDetectionSide6;

	public int diceResult = 0;

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
        rayDetectionSide1 = CreateSideRayCast(Vector3.down);
        rayDetectionSide2 = CreateSideRayCast(Vector3.left);
        rayDetectionSide3 = CreateSideRayCast(Vector3.forward);
        rayDetectionSide4 = CreateSideRayCast(Vector3.back);
        rayDetectionSide5 = CreateSideRayCast(Vector3.right);
        rayDetectionSide6 = CreateSideRayCast(Vector3.up);

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

    Ray CreateSideRayCast(Vector3 dir) {
		return new Ray(_meshTransform.position, (_meshTransform.rotation * dir).normalized * 5f);
    }

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player") {
			//bring random menu if the die result is set
			dieMenu.Activate(diceResult);
		}
	}

	public void KillAfterDelay(float timeBeforeKill = 20f)
	{
		Destroy(gameObject, timeBeforeKill);
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

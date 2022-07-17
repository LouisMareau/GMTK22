using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	public DieMenu dieMenu;
	public new Rigidbody rigidbody;

	void Start()
	{
		if (dieMenu == null) dieMenu = this.GetComponentInChildren<DieMenu>();
		if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();

		rigidbody.isKinematic = true;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player") {
			//bring random menu if the die result is set
			dieMenu.Launch();
		}
	}
}

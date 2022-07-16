using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
	public DieMenu dieMenu;
	// Start is called before the first frame update
	void Start()
	{
		if (dieMenu == null) dieMenu = this.GetComponentInChildren<DieMenu>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	//void OnCollisionEnter(Collision collision)
	//{
	//    foreach (ContactPoint contact in collision.contacts) {
	//        if (contact.otherCollider.tag == "Player") {
	//            //bring random menu
	//            dieMenu.Launch();
	//        }

	//    }
	//}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			//bring random menu
			dieMenu.Launch();
		}
	}

	
}

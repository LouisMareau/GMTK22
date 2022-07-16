using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public GameObject dieMenu;
    // Start is called before the first frame update
    void Start()
    {
        dieMenu = GameObject.Find("dieMenu");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts) {
            if (contact.otherCollider.name == "Player") {
                //bring menu
                //adad


            }

        }
    }

    
}

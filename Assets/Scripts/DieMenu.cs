using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer test = GetComponent<MeshRenderer>();
        test.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Launch() {
        //pause the game
        Time.timeScale = 0f; 

        //display
        MeshRenderer test = GetComponent<MeshRenderer>();
        test.enabled = true;

        //generate number between 1 and 6
        int die_throw_result = Random.Range(1, 6);
        
    }
}


public class MenuOption {
    int bonus_index;
    PlayerController player;


    public MenuOption(int die_throw_result)  {
        bonus_index = die_throw_result;
        //player = player;
        

    }


}

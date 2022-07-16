using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionButton : MonoBehaviour
{
    PlayerData playerData;
    System.Action<PlayerData> action;

	// Start is called before the first frame update
	void Start()
    {
        //find player data and init
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TaskOnClick() {
        action(playerData);

        //hide menu
        Renderer test = this.transform.root.GetComponent<Renderer>();
        test.enabled = false;

        //resume game
        Time.timeScale = 1f; 
    }


    public void setAction(System.Action<PlayerData> action) {
        this.action = action;
    }
}

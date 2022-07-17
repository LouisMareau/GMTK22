using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOptionButton : MonoBehaviour
{
	[SerializeField] private GameObject _dieRoot;

	PlayerData playerData;
	System.Action<PlayerData> action;
	public Button b;
	public TextMeshProUGUI bText;

	// Start is called before the first frame update
	void Start()
	{
		//find player data and init
		playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void TaskOnClick() {
		action(playerData);

		//resume game
		Time.timeScale = 1f;
		// Destroy the die
		Destroy(_dieRoot);
	}

	public void setText(string text) {
		bText.text = text;
	}


	public void setAction(System.Action<PlayerData> action) {
		this.action = action;
	}
}

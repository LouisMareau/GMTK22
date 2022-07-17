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
	public TextMeshProUGUI bID;

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

    public void setup((string, System.Action<PlayerData>) effect, int die_throw_result) {
        setIDtext(die_throw_result.ToString());
        setText(effect.Item1);
        setAction(effect.Item2);
    }

	void setIDtext(string text) {
		bID.text = text;
	}
	void setText(string text) {
		bText.text = text;
	}
	void setAction(System.Action<PlayerData> action) {
		this.action = action;
	}
}

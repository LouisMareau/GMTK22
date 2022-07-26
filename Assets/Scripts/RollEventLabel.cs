using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollEventLabel : MonoBehaviour
{
	[SerializeField] private DieMenu _associatedMenu;
	[SerializeField] private Button _button;
	[Space]
	[SerializeField] private TextMeshProUGUI _title;
	[SerializeField] private TextMeshProUGUI _description;

	private RollEffect _rollEffect;

	#region ON ENABLE/DISABLE
	private void OnEnable()
	{
		_button.onClick.AddListener(Activate);
	}

	private void OnDisable()
	{
		_button.onClick.RemoveAllListeners();
	}
	#endregion

	#region INITIALIZATION
	public void Initialize(DieMenu menu, RollEffect rollEffect)
	{
		_associatedMenu = menu;
		_rollEffect = rollEffect;
		_title.text = rollEffect.name;
		_description.text = rollEffect.description;
	}
	#endregion

	private void Activate()
	{
		_rollEffect.Activate();

		// We resume the game
		Time.timeScale = 1f;

		// We destroy the die
		_associatedMenu._associatedDie.Kill();
        //We hide the menu
        _associatedMenu.Hide();
	}
}

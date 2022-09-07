using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RollEventLabel : MonoBehaviour
{
	[SerializeField] private DiceHUD _associatedMenu;
	[SerializeField] private Button _button;
	[Space]
	[SerializeField] private TextMeshProUGUI _title;
	[SerializeField] private TextMeshProUGUI _description;
	[SerializeField] private RectTransform _descriptionRT;
	[Space]
	[SerializeField] private Image _colorBand;
	[SerializeField] private Color _colorBandBonus;
	[SerializeField] private Color _colorBandMalus;
	[SerializeField] private Color _colorBandNeutral;

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
	public void Initialize(DiceHUD menu, RollEffect rollEffect)
	{
		_associatedMenu = menu;
		_rollEffect = rollEffect;
		_title.text = rollEffect.name;
		_description.text = rollEffect.description;

		switch (rollEffect.behaviour)
		{
			case EffectBehaviour.BONUS:
				_colorBand.color = _colorBandBonus;
				break;
			case EffectBehaviour.MALUS:
				_colorBand.color = _colorBandMalus;
				break;
			case EffectBehaviour.NEUTRAL:
				_colorBand.color = _colorBandNeutral;
				break;
		}
	}
	#endregion

	private void Activate()
	{
		_rollEffect.Activate();
		
		// We notify the player
		DiceHUD.Instance.Notify(_rollEffect);

		// We resume the game
		//Time.timeScale = 1f;
		GameManager.SwitchGameState(GameState.PLAY);

		// We destroy the dice
		if (_associatedMenu._associatedDice != null)
			_associatedMenu._associatedDice.Kill();

        //We hide the menu
        _associatedMenu.Hide();
	}
}

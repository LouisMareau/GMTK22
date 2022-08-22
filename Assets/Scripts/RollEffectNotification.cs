using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollEffectNotification : MonoBehaviour
{
	[SerializeField] private Color colorBonus;
	[SerializeField] private Color colorMalus;
	[SerializeField] private Color colorNeutral;
	[Space]
	[SerializeField] private float _duration = 3.0f;


	[Header("LOCAL REFERENCES")]
	[SerializeField] private TextMeshProUGUI _label;
	[SerializeField] private Image _background;
	
	public void Init(RollEffect effect) { StartCoroutine(Init_Coroutine(effect)); }
	private IEnumerator Init_Coroutine(RollEffect effect)
	{
		switch (effect.behaviour)
		{
			case EffectBehaviour.BONUS:
				_background.color = colorBonus;
				break;
			case EffectBehaviour.MALUS:
				_background.color = colorMalus;
				break;
			case EffectBehaviour.NEUTRAL:
				_background.color = colorNeutral;
				break;
		}
		
		_label.text = $"• { effect.name } •";

		float timer = 0.0f;
		while (timer < _duration)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}
}
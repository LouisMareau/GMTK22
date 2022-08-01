using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RollEventLabel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private DieMenu _associatedMenu;
	[SerializeField] private Button _button;
	[Space]
	[SerializeField] private TextMeshProUGUI _title;
	[SerializeField] private TextMeshProUGUI _description;
	[SerializeField] private RectTransform _descriptionRT;


	[Header("ANIMATIONS")]
	[SerializeField] private Vector2 _descriptionContainerMinMaxHeight = new Vector2(0.0f, 120.0f);
	[SerializeField] private float _showEffectDescriptionDuration = 0.4f;
	[SerializeField] private float _hideEffectDescriptionDuration = 0.4f;
	[SerializeField] private float _fadeInEffectDescriptionDuration = 0.2f;
	[SerializeField] private float _fadeInEffectDescriptionDelay = 0.2f;
	[SerializeField] private float _fadeOutEffectDescriptionDuration = 0.2f;
	[SerializeField] private float _fadeOutEffectDescriptionDelay = 0.2f;

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
	private void Awake()
	{
		_descriptionRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
		_description.color = new Color32(255, 255, 255, 0);
	}

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
		//Time.timeScale = 1f;
		GameManager.SwitchGameState(GameState.PLAY);

		// We destroy the die
		_associatedMenu._associatedDie.Kill();
        //We hide the menu
        _associatedMenu.Hide();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		StopCoroutine(ShowEffectDescription_Coroutine());
		StopCoroutine(HideEffectDescription_Coroutine());
		StopCoroutine(FadeInEffectDescription_Coroutine());
		StopCoroutine(FadeOutEffectDescription_Coroutine());

		ShowEffectDescription();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopCoroutine(ShowEffectDescription_Coroutine());
		StopCoroutine(HideEffectDescription_Coroutine());
		StopCoroutine(FadeInEffectDescription_Coroutine());
		StopCoroutine(FadeOutEffectDescription_Coroutine());

		HideEffectDescription();
	}

	public void ShowEffectDescription() { StartCoroutine(ShowEffectDescription_Coroutine()); }
	private IEnumerator ShowEffectDescription_Coroutine()
	{
		float timer = 0.0f;
		while (timer < _showEffectDescriptionDuration)
		{
			float height = Mathf.Lerp(_descriptionContainerMinMaxHeight.x, _descriptionContainerMinMaxHeight.y, timer / _showEffectDescriptionDuration);
			_descriptionRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

			if (timer > _fadeInEffectDescriptionDelay)
				FadeInEffectDescription();

			timer += Time.deltaTime;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
	}

	public void HideEffectDescription() { StartCoroutine(HideEffectDescription_Coroutine()); }
	private IEnumerator HideEffectDescription_Coroutine()
	{
		float timer = 0.0f;
		while (timer < _hideEffectDescriptionDuration)
		{
			float height = Mathf.Lerp(_descriptionContainerMinMaxHeight.y, _descriptionContainerMinMaxHeight.x, timer / _showEffectDescriptionDuration);
			_descriptionRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

			if (timer > _fadeOutEffectDescriptionDelay)
				FadeOutEffectDescription();

			timer += Time.deltaTime;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
	}

	public void FadeInEffectDescription() { StartCoroutine(FadeInEffectDescription_Coroutine()); }
	private IEnumerator FadeInEffectDescription_Coroutine()
	{
		float timer = 0.0f;
		while (timer < _fadeInEffectDescriptionDuration)
		{
			Color32 color = Color32.Lerp(new Color32(255, 255, 255, 0), new Color32(255, 255, 255, 255), timer / _fadeInEffectDescriptionDuration);
			_description.color = color;

			timer += Time.deltaTime;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
	}

	public void FadeOutEffectDescription() { StartCoroutine(FadeOutEffectDescription_Coroutine()); }
	private IEnumerator FadeOutEffectDescription_Coroutine()
	{
		float timer = 0.0f;
		while (timer < _fadeOutEffectDescriptionDuration)
		{
			Color32 color = Color32.Lerp(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 0), timer / _fadeOutEffectDescriptionDuration);
			_description.color = color;

			timer += Time.deltaTime;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
	}
}

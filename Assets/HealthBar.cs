using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Image _fillRef;
	[SerializeField] [Range(0.0f, 1.0f)] private float _fillAmount;

	#region IN-EDITOR
	private void OnValidate()
	{
		if (_fillRef != null)
			_fillRef.fillAmount = _fillAmount;
	}
	#endregion

	#region INITIALIZATION
	private void Awake()
	{
		_fillRef.fillAmount = _fillAmount = 0;
	}
	#endregion

	public void FillInstant() { _fillRef.fillAmount = _fillAmount = 1.0f; }
	public IEnumerator Fill(float duration) { yield return StartCoroutine(Fill_Coroutine(duration)); }
	public IEnumerator Fill_Coroutine(float duration)
	{
		float startValue = _fillAmount;
		float endValue = 1.0f;

		float timer = 0.0f;
		while (timer < duration)
		{
			_fillAmount = Mathf.Lerp(startValue, endValue, timer / duration);
			_fillRef.fillAmount = _fillAmount;

			timer += Time.deltaTime;
			yield return null;
		}

		if (_fillAmount != 1.0f)
			_fillRef.fillAmount = _fillAmount = 1.0f;
	}

	public void EmptyInstant() { _fillRef.fillAmount = _fillAmount = 0.0f; }
	public IEnumerator Empty(float duration) { yield return StartCoroutine(Empty_Coroutine(duration)); }
	public IEnumerator Empty_Coroutine(float duration)
	{
		float startValue = _fillAmount;
		float endValue = 0.0f;

		float timer = 0.0f;
		while (timer < duration)
		{
			_fillAmount = Mathf.Lerp(startValue, endValue, timer / duration);
			_fillRef.fillAmount = _fillAmount;

			timer += Time.deltaTime;
			yield return null;
		}

		if (_fillAmount != 0.0f)
			_fillRef.fillAmount = _fillAmount = 0.0f;
	}
}
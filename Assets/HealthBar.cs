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

	public IEnumerator Fill(float duration) { yield return StartCoroutine(Fill_Coroutine(duration)); }
	public IEnumerator Fill_Coroutine(float duration)
	{
		float timer = 0.0f;
		while (timer < duration)
		{
			_fillAmount = Mathf.Lerp(0.0f, 1.0f, timer / duration);
			_fillRef.fillAmount = _fillAmount;

			timer += Time.deltaTime;
			yield return null;
		}

		if (_fillAmount != 1.0f)
			_fillRef.fillAmount = _fillAmount = 1.0f;
	}

	public IEnumerator Empty(float duration) { yield return StartCoroutine(Empty_Coroutine(duration)); }
	public IEnumerator Empty_Coroutine(float duration)
	{
		float timer = 0.0f;
		while (timer < duration)
		{
			_fillAmount = Mathf.Lerp(1.0f, 0.0f, timer / duration);
			_fillRef.fillAmount = _fillAmount;

			timer += Time.deltaTime;
			yield return null;
		}

		if (_fillAmount != 0.0f)
			_fillRef.fillAmount = _fillAmount = 0.0f;
	}
}
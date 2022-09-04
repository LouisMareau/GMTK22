using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarMultiplier : MonoBehaviour
{
	public TextMeshProUGUI label;
	public GameObject root;

	public bool IsVisible { get { return root.activeInHierarchy; } }
	public void Show() { if (!IsVisible) { root.SetActive(true); } }
	public void Hide() { if (IsVisible) { root.SetActive(false); } }

	public void SetLabel(float amount)
	{
		if (HealthManager.Health <= 10)
			Hide();
		else
		{
			Show();
			label.text = $"x{ Mathf.FloorToInt(amount) }";
		}
	}
}
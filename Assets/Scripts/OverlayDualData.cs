using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayDualData : MonoBehaviour
{
	[SerializeField] private string _title;
	[SerializeField] private int _valueX;
	[SerializeField] private int _valueY;
	[Space]
	[SerializeField] private Color _backgroundColor;

	[Header("LOCAL REFERENCES")]
	[SerializeField] private GameObject _root;
	[SerializeField] private GameObject _titleRoot;
	[SerializeField] private GameObject _valueXRoot;
	[SerializeField] private GameObject _valueYRoot;
	[Space]
	[SerializeField] private TextMeshProUGUI _titleLabel;
	[SerializeField] private TextMeshProUGUI _valueXLabel;
	[SerializeField] private TextMeshProUGUI _valueYLabel;

	private Image _titleBackground;
	private Image _valueXBackground;
	private Image _valueYBackground;

	#region EDITOR
	private void OnValidate()
	{
		if (_titleBackground == null) { _titleBackground = _titleRoot.GetComponent<Image>(); }
		if (_valueXBackground == null) { _valueXBackground = _valueXRoot.GetComponent<Image>(); }
		if (_valueYBackground == null) { _valueYBackground = _valueYRoot.GetComponent<Image>(); }

		if (_root != null) { _root.name = $"[] {_title}"; }
		if (_titleRoot != null) { _titleRoot.name = $"{_title} Title"; }
		if (_valueXRoot != null) { _valueXRoot.name = $"{_title} Value X"; }
		if (_valueYRoot != null) { _valueYRoot.name = $"{_title} Value Y"; }
		if (_titleBackground != null) { _titleBackground.color = _backgroundColor; }
		if (_valueXBackground != null) { _valueXBackground.color = _backgroundColor; }
		if (_valueYBackground != null) { _valueYBackground.color = _backgroundColor; }

		Set(_title, _valueX, _valueY);
	}
	#endregion

	public void Set(string title, float valueX, float valueY)
	{
		this._title = title;
		this._valueX = (int)valueX;
		this._valueY = (int)valueY;

		if (_titleLabel != null) { _titleLabel.text = _title; }
		if (_valueXLabel != null) { _valueXLabel.text = _valueX.ToString("F1"); }
		if (_valueYLabel != null) { _valueYLabel.text = _valueY.ToString("F1"); }
	}

	public void Set(string title, int valueX, int valueY)
	{
		this._title = title;
		this._valueX = valueX;
		this._valueY = valueY;

		if (_titleLabel != null) { _titleLabel.text = _title; }
		if (_valueXLabel != null) { _valueXLabel.text = _valueX.ToString(); }
		if (_valueYLabel != null) { _valueYLabel.text = _valueY.ToString(); }
	}
}
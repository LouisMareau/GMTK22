using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayData : MonoBehaviour
{
	[SerializeField] private string _title;
	[SerializeField] private int _value;
	[Space]
	[SerializeField] private Color _backgroundColor;

	[Header("LOCAL REFERENCES")]
	[SerializeField] private GameObject _root;
	[SerializeField] private GameObject _titleRoot;
	[SerializeField] private GameObject _valueRoot;
	[Space]
	[SerializeField] private TextMeshProUGUI _titleLabel;
	[SerializeField] private TextMeshProUGUI _valueLabel;

	private Image _titleBackground;
	private Image _valueBackground;

	#region EDITOR
	private void OnValidate()
	{
		if (_titleBackground == null) { _titleBackground = _titleRoot.GetComponent<Image>(); }
		if (_valueBackground == null) { _valueBackground = _valueRoot.GetComponent<Image>(); }

		if (_root != null) { _root.name = $"[] {_title}"; }
		if (_titleRoot != null) { _titleRoot.name = $"{_title} Title"; }
		if (_valueRoot != null) { _valueRoot.name = $"{_title} Value"; }
		if (_titleBackground != null) { _titleBackground.color = _backgroundColor; }
		if (_valueBackground != null) { _valueBackground.color = _backgroundColor; }

		Set(_title, _value);
	}
	#endregion

	public void Set(string title, float value)
	{
		this._title = title;
		this._value = (int)value;

		if (_titleLabel != null) { _titleLabel.text = _title; }
		if (_valueLabel != null) { _valueLabel.text = _value.ToString("F1"); }
	}

	public void Set(string title, int value)
	{
		this._title = title;
		this._value = value;

		if (_titleLabel != null) { _titleLabel.text = _title; }
		if (_valueLabel != null) { _valueLabel.text = _value.ToString(); }
	}
}

[System.Serializable]
public class HUDOverlayData
{
	public TextMeshProUGUI timer;
	[Space]
	public OverlayData playerSpeed;
	public OverlayData playerDamage;
	public OverlayData playerFirerateStandard;
	public OverlayData playerFirerateSeeker;
	[Space]
	public OverlayDualData enemyMDRandomSpawnInterval;
	public OverlayData enemyMDHealth;
	public OverlayData enemyMDSpeed;
	public OverlayData enemyMDDamage;
	public OverlayData enemyMDScoreOnKill;
	public OverlayData enemyMDRotationSpeed;
	[Space]
	public OverlayDualData enemyDHRandomSpawnInterval;
	public OverlayData enemyDHHealth;
	public OverlayData enemyDHSpeed;
	public OverlayData enemyDHDamage;
	public OverlayData enemyDHScoreOnKill;
	public OverlayData enemyDHRotationSpeed;
	[Space]
	public OverlayDualData enemyPRandomSpawnInterval;
	public OverlayData enemyPHealth;
	public OverlayData enemyPSpeed;
	public OverlayData enemyPDamage;
	public OverlayData enemyPScoreOnKill;
	public OverlayData enemyPRotationSpeed;
	public OverlayData enemyPChargeDuration;
	public OverlayData enemyPHoldDuration;
	public OverlayData enemyPBlastDuration;
	public OverlayData enemyPCooldownDuration;
}
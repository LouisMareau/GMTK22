using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieHUD : MonoBehaviour
{
	#region SINGLETON
	public static DieHUD Instance { get; private set; }
	#endregion

	[HideInInspector] public Die _associatedDie;

	[SerializeField] private GameObject _dicePickUpMenu;
	[SerializeField] private TextMeshProUGUI _resultLabel;
	[Space]
	[SerializeField] private RollEventLabel _eventLabel1;
	[SerializeField] private RollEventLabel _eventLabel2;
	[SerializeField] private RollEventLabel _eventLabel3;

	[Header("PREFABS")]
	[SerializeField] private GameObject _prefabNotification;

	#region INITIALIZATION
	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		if (IsVisible) Hide();
	}

	public void SetAssociatedDie(Die die) { _associatedDie = die; }
	#endregion

	#region VISIBILITY
	public void Show() { _dicePickUpMenu.SetActive(true); }
	public void Hide() { _dicePickUpMenu.SetActive(false); }
	public bool IsVisible { get { return _dicePickUpMenu.activeInHierarchy; } }
	#endregion

	#region GAMEPLAY
	public void ActivateDie(int result)
	{
		// We show the menu
		Show();

		// We update the label on the menu
		_resultLabel.text = result.ToString();

		// We populate the list of effect the player can choose from
		List<RollEffect> effects = DiceManager.Effects.GetEffectsOnActivation(result);
		_eventLabel1.Initialize(this, effects[0]);
		_eventLabel2.Initialize(this, effects[1]);
		_eventLabel3.Initialize(this, effects[2]);
	}

	public void Notify(RollEffect effect)
	{
		RollEffectNotification notif = Instantiate<GameObject>(_prefabNotification, StaticReferences.Instance.notificationContainer).GetComponent<RollEffectNotification>();
		notif.Init(effect);
	}
	#endregion
}
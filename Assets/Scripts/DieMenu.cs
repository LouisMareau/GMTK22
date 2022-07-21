using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieMenu : MonoBehaviour
{

	public Die6 _assocaitedDie;
	[SerializeField] private Canvas _canvas;
	[SerializeField] private TextMeshProUGUI _resultLabel;
	[Space]
	[SerializeField] private RollEventLabel _eventLabel1;
	[SerializeField] private RollEventLabel _eventLabel2;
	[SerializeField] private RollEventLabel _eventLabel3;

	public Dictionary<RollEffectType, RollEffect> RollEffectsMaps { get; private set; }

	#region INITIALIZATION
	private void Awake()
	{
		if (RollEffectsMaps == null) { RollEffectsMaps = new Dictionary<RollEffectType, RollEffect>(); }

		InitializeRollEffects();
	}

	private void InitializeRollEffects()
	{
		RollEffectsMaps.Add(RollEffectType.POWER_UP, new RollEffect_PowerUp());
		RollEffectsMaps.Add(RollEffectType.POWER_UP_PLUS, new RollEffect_PowerUpPlus());
		RollEffectsMaps.Add(RollEffectType.KILLING_FRENZY, new RollEffect_KillingFrenzy());
		RollEffectsMaps.Add(RollEffectType.JUGEMENT_DAY, new RollEffect_JugementDay());
	}

	private void Start()
	{
		if (IsVisible) Hide();
	}
	#endregion

	public void Activate(int result)
	{
		if (result != 0)
		{
			// We pause the game
			Time.timeScale = 0.0f;

			// We show the menu
			Show();

			// We update the label on the menu
			_resultLabel.text = result.ToString();

			// We populate the list of effect the player can choose from
			var effect1 = RollEffectsMaps[RollEffectType.POWER_UP];
			var effect2 = RollEffectsMaps[RollEffectType.KILLING_FRENZY];
			var effect3 = RollEffectsMaps[RollEffectType.JUGEMENT_DAY];
			_eventLabel1.Initialize(this, effect1);
			_eventLabel2.Initialize(this, effect2);
			_eventLabel3.Initialize(this, effect3);
		}
	}

	#region VISIBILITY
	public void Show() { _canvas.enabled = true; }
	public void Hide() { _canvas.enabled = false; }
	public bool IsVisible { get { return _canvas.enabled; } }
	#endregion

	
}

#region ROLL EFFECTS
public abstract class RollEffect
{
	public int id;
	public string name;
	public string description;
	public abstract void Activate();
}

#region EFFECTS
public class RollEffect_PowerUp : RollEffect
{
	public RollEffect_PowerUp()
	{
		id = (int)RollEffectType.POWER_UP;
		name = "Power Up";
		description = "You gain +1 life.";
	}

	public override void Activate()
	{
		RollEffectsDefinition.Instance.PowerUp();
	}
}
public class RollEffect_PowerUpPlus : RollEffect
{
	public RollEffect_PowerUpPlus()
	{
		id = (int)RollEffectType.POWER_UP_PLUS;
		name = "Power Up+";
		description = "You gain +2 lives.";
	}

	public override void Activate()
	{
		RollEffectsDefinition.Instance.PowerUpPlus();
	}
}
public class RollEffect_KillingFrenzy : RollEffect
{
	public RollEffect_KillingFrenzy()
	{
		id = (int)RollEffectType.KILLING_FRENZY;
		name = "Killing Frenzy";
		description = "For the next 15 seconds, killing 7+ ennemies in less than 2s will grant +1 life (stackable 3 times).";
	}

	public override void Activate()
	{
		RollEffectsDefinition.Instance.KillingFrenzy();
	}
}
public class RollEffect_JugementDay : RollEffect
{
	public RollEffect_JugementDay()
	{
		id = (int)RollEffectType.JUGEMENT_DAY;
		name = "Jugement Day";
		description = "For the next 15 seconds, killing 3+ ennemies in less than 1.5s will deal -1 life (unlimited stacking).";
	}

	public override void Activate()
	{
		RollEffectsDefinition.Instance.JugementDay();
	}
}
#endregion

public enum RollEffectType
{
	POWER_UP,
	POWER_UP_PLUS,
	KILLING_FRENZY,
	JUGEMENT_DAY,
	NOT_TODAY,
	ROLL_WITH_THE_FLOW,
	JUST_NEEDED_SOME_GREASE,
	SACREBLUE_ITS_JAMMED_AGAIN,
	MAGIC_FINGERS,
	I_WENT_TO_THE_SHOOTING_RANGE,
	AMERICAN_SNIPER,
	FEAR_OF_DAMAGING_GOODS,
	EAGLE_EYE,
	TIME_TO_MAKE_PEACE,
	IS_THIS_MAGIC,
	LOOK_AT_MY_NEW_GADGET,
	DESTABILITATING_SHOTS,
	DID_I_BUY_RUBBER_BULLETS,
	FASTER_THAN_A_BULL_ETTTE,
	BULLET_HELL,
	NOT_YOUR_GRANDPAS_AMMO,
	SLIPPERY_FLOORS
}
#endregion
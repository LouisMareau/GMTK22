using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieMenu : MonoBehaviour
{
	#region SINGLETON
	public static DieMenu Instance { get; private set; }
	#endregion

	[HideInInspector] public Die6 _associatedDie;

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
		Instance = this;

		if (RollEffectsMaps == null) { RollEffectsMaps = new Dictionary<RollEffectType, RollEffect>(); }
	}

	private void Start()
	{
		if (IsVisible) Hide();

		InitializeRollEffects();
	}

	public void SetAssociatedDie(Die6 die)
	{
		_associatedDie = die;
	}

	private void InitializeRollEffects()
	{
		// LIFE EVENTS
		AddRollEffect(
				RollEffectType.POWER_UP,
				"Power Up",
				"You gain +1 life",
				RollEffectsDefinition.Instance.PowerUp
		);
		AddRollEffect(
				RollEffectType.POWER_UP_PLUS,
				"Power Up+",
				"You gain +2 lives",
				RollEffectsDefinition.Instance.PowerUpPlus
		);
		AddRollEffect(
				RollEffectType.KILLING_FRENZY,
				"Killing Frenzy",
				"For the next 15 seconds, killing 7+ ennemies in less than 2s will grant +1 life (stackable 3 times)",
				RollEffectsDefinition.Instance.KillingFrenzy
		);
		AddRollEffect(
				RollEffectType.JUDGEMENT_DAY,
				"Judgement Day",
				"For the next 15 seconds, killing 3+ ennemies in less than 1.5s will deal -1 life (unlimited stacking)",
				RollEffectsDefinition.Instance.JudgementDay
		);
		AddRollEffect(
				RollEffectType.NOT_TODAY,
				"Not Today!",
				"For the next 60 seconds, if you were to be dealt lethal damage once, you will survive",
				RollEffectsDefinition.Instance.NotToday
		);

		// -------------------------------
		// SHOOTING EVENTS
		AddRollEffect(
				RollEffectType.ROLL_WITH_THE_FLOW,
				"Roll with the flow",
				"Increases the fire rate by 1 projectile/s",
				RollEffectsDefinition.Instance.RollWithTheFlow
		);
		AddRollEffect(
				RollEffectType.JUST_NEEDED_SOME_GREASE,
				"Just needed some grease...",
				"Increases the fire rate by 2 projectiles/s",
				RollEffectsDefinition.Instance.JustNeededSomeGrease
		);
		AddRollEffect(
				RollEffectType.SACREBLUE_ITS_JAMMED_AGAIN,
				"Sacrebleu! It's jammed again!",
				"Decreases the fire rate by 1 projectile/s",
				RollEffectsDefinition.Instance.SacrebleuItsJammedAgain
		);
		AddRollEffect(
				RollEffectType.MAGIC_FINGERS,
				"Magic fingers",
				"For the next 10 seconds, multiplies the current fire rate by 2 (delays any new increase until the end of the effect)",
				RollEffectsDefinition.Instance.MagicFingers
		);
		AddRollEffect(
				RollEffectType.I_WENT_TO_THE_SHOOTING_RANGE,
				"I went to the shooting range!",
				"Increases the damage output by 0.5",
				RollEffectsDefinition.Instance.IWentToTheShootingRange
		);
		AddRollEffect(
				RollEffectType.AMERICAN_SNIPER,
				"American Sniper",
				"Increases the damage output by 1",
				RollEffectsDefinition.Instance.AmericanSniper
		);
		AddRollEffect(
				RollEffectType.FEAR_OF_DAMAGING_GOODS,
				"Fear of damaging goods",
				"Decrease the damage output by 0.5",
				RollEffectsDefinition.Instance.FearOfDamagingGoods
		);
		AddRollEffect(
				RollEffectType.EAGLE_EYE,
				"Eagle Eye",
				"For the next 10 seconds, multiplies the current damage ouput by 2 (delays any new increase until the end of the effect)",
				RollEffectsDefinition.Instance.EagleEye
		);
		AddRollEffect(
				RollEffectType.TIME_TO_MAKE_PEACE,
				"Time to make peace.",
				"For the next 20 seconds, you cannot use your weapon",
				RollEffectsDefinition.Instance.TimeToMakePeace
		);
		AddRollEffect(
				RollEffectType.IS_THIS_MAGIC,
				"Is this magic?!",
				"For the next 10 seconds, the projectiles are seeking enemies (closest one)",
				RollEffectsDefinition.Instance.IsThisMagic
		);
		AddRollEffect(
				RollEffectType.LOOK_AT_MY_NEW_GADGET,
				"Look at my new gadget!",
				"Transforms 1 projectile into a seeker",
				RollEffectsDefinition.Instance.LookAtMyNewGadget
		);
		AddRollEffect(
				RollEffectType.DESTABILITATING_SHOTS,
				"Destabilitating shots",
				"Knockback +1",
				RollEffectsDefinition.Instance.DestabilitatingShots
		);
		AddRollEffect(
				RollEffectType.DID_I_BUY_RUBBER_BULLETS,
				"Did I buy rubber bullets?!",
				"Knockback -1",
				RollEffectsDefinition.Instance.DidIBuyRubberBullets
		);
		AddRollEffect(
				RollEffectType.BULLET_HELL,
				"Bullet Hell!",
				"Projectile spread +1",
				RollEffectsDefinition.Instance.BulletHell
		);
		AddRollEffect(
				RollEffectType.NOT_YOUR_GRANDPAS_AMMO,
				"Not your grandpa's ammo...",
				"Projectile size +10%",
				RollEffectsDefinition.Instance.NotYourGrandpasAmmo
		);

		// -------------------------------
		// ENEMY EVENTS

		// [TO DO] ...
	}

	private void AddRollEffect(RollEffectType type, string name, string description, System.Action Activate)
	{
		RollEffectsMaps.Add(type, new RollEffect(type, name, description, Activate));
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
			var effect1 = RollEffectsMaps[RollEffectType.ROLL_WITH_THE_FLOW];
			var effect2 = RollEffectsMaps[RollEffectType.JUST_NEEDED_SOME_GREASE];
			var effect3 = RollEffectsMaps[RollEffectType.LOOK_AT_MY_NEW_GADGET];
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
public class RollEffect
{
	public int id;
	public string name;
	public string description;
	public System.Action Activate;

	public RollEffect(RollEffectType type, string name, string description, System.Action Activate) {
		this.id = (int) type;
		this.name = name;
		this.description = description;
		this.Activate = Activate;
	}
}

public enum RollEffectType
{
	POWER_UP,								// OK
	POWER_UP_PLUS,							// OK
	KILLING_FRENZY,							// OK
	JUDGEMENT_DAY,							// OK
	NOT_TODAY,								// OK
	ROLL_WITH_THE_FLOW,						// OK
	JUST_NEEDED_SOME_GREASE,				// OK
	SACREBLUE_ITS_JAMMED_AGAIN,				// OK
	MAGIC_FINGERS,							// OK
	I_WENT_TO_THE_SHOOTING_RANGE,			// OK
	AMERICAN_SNIPER,						// OK
	FEAR_OF_DAMAGING_GOODS,					// OK
	EAGLE_EYE,								// OK
	TIME_TO_MAKE_PEACE,						// OK
	IS_THIS_MAGIC,
	LOOK_AT_MY_NEW_GADGET,					// OK
	I_SPEC_INTO_AERODYNAMISM,
	JUST_LIKE_THE_GOOD_OLD_TRUSTY_ARROW,
	DESTABILITATING_SHOTS,
	DID_I_BUY_RUBBER_BULLETS,
	FASTER_BULLETS,
	BULLET_HELL,
	NOT_YOUR_GRANDPAS_AMMO,
	SLIPPERY_FLOORS,
	HE_IS_OVER_9000,
	WRONG_MUTATION,
	CHRISTMAS_SHOPPING,
	I_WISH_I_GOT_THAT_BUFF,
	WRONG_MUTATION_V2,
	TRAFFIC_JAM,
	THEY_HATE_MONDAYS_TOO,
	I_HOPE_THATS_NOT_YOUR_FIRST_PICK,
	DUPLICATION_TACTICS,
	THEY_FOUND_A_CHEAT,
	BOOMERS
}

#region ARCHIVES
//#region EFFECTS
//public class RollEffect_PowerUp : RollEffect
//{
//public RollEffect_PowerUp()
//{
//id = (int)RollEffectType.POWER_UP;
//name = "Power Up";
//description = "You gain +1 life.";
//}

//public override void Activate()
//{
//RollEffectsDefinition.Instance.PowerUp();
//}
//}
//public class RollEffect_PowerUpPlus : RollEffect
//{
//public RollEffect_PowerUpPlus()
//{
//id = (int)RollEffectType.POWER_UP_PLUS;
//name = "Power Up+";
//description = "You gain +2 lives.";
//}

//public override void Activate()
//{
//RollEffectsDefinition.Instance.PowerUpPlus();
//}
//}
//public class RollEffect_KillingFrenzy : RollEffect
//{
//public RollEffect_KillingFrenzy()
//{
//id = (int)RollEffectType.KILLING_FRENZY;
//name = "Killing Frenzy";
//description = "For the next 15 seconds, killing 7+ ennemies in less than 2s will grant +1 life (stackable 3 times).";
//}

//public override void Activate()
//{
//RollEffectsDefinition.Instance.KillingFrenzy();
//}
//}
//public class RollEffect_JugementDay : RollEffect
//{
//public RollEffect_JugementDay()
//{
//id = (int)RollEffectType.JUGEMENT_DAY;
//name = "Jugement Day";
//description = "For the next 15 seconds, killing 3+ ennemies in less than 1.5s will deal -1 life (unlimited stacking).";
//}

//public override void Activate()
//{
//RollEffectsDefinition.Instance.JugementDay();
//}
//}
//#endregion
#endregion

#endregion

using System.Collections.Generic;
using UnityEngine;

public class RollEffects
{
	public Dictionary<RollEffectType, RollEffect> RollEffectsAssociation { get; private set; }
	public Dictionary<int, RollMap> RollMaps { get; private set; }

	public RollEffects()
	{
		if (RollEffectsAssociation == null) { RollEffectsAssociation = new Dictionary<RollEffectType, RollEffect>(); }
		if (RollMaps == null) { RollMaps = new Dictionary<int, RollMap>(); }

		MapRollEffects();
	}

	private void AssociateRollEffects()
	{
		// LIFE EVENTS
		AddRollEffect(
			RollEffectType.POWER_UP,
			"Power Up",
			"You gain +1 life",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.PowerUp
		);
		AddRollEffect(
			RollEffectType.POWER_UP_PLUS,
			"Power Up+",
			"You gain +2 lives",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.PowerUpPlus
		);
		AddRollEffect(
			RollEffectType.KILLING_FRENZY,
			"Killing Frenzy",
			"For the next 15 seconds, killing 7+ ennemies in less than 2s will grant +1 life (stackable 3 times)",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.KillingFrenzy
		);
		AddRollEffect(
			RollEffectType.JUDGEMENT_DAY,
			"Judgement Day",
			"For the next 15 seconds, killing 3+ ennemies in less than 1.5s will deal -1 life (unlimited stacking)",
			EffectBehaviour.MALUS,
			RollEffectsDefinition.Instance.JudgementDay
		);
		AddRollEffect(
			RollEffectType.NOT_TODAY,
			"Not Today!",
			"For the next 60 seconds, if you were to be dealt lethal damage once, you will survive",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.NotToday
		);

		// -------------------------------
		// SHOOTING EVENTS
		AddRollEffect(
			RollEffectType.ROLL_WITH_THE_FLOW,
			"Roll with the flow",
			"Increases the fire rate by 1 projectile/s",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.RollWithTheFlow
		);
		AddRollEffect(
			RollEffectType.JUST_NEEDED_SOME_GREASE,
			"Just needed some grease...",
			"Increases the fire rate by 2 projectiles/s",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.JustNeededSomeGrease
		);
		AddRollEffect(
			RollEffectType.SACREBLUE_ITS_JAMMED_AGAIN,
			"Sacrebleu! It's jammed again!",
			"Decreases the fire rate by 1 projectile/s",
			EffectBehaviour.MALUS,
			RollEffectsDefinition.Instance.SacrebleuItsJammedAgain
		);
		AddRollEffect(
			RollEffectType.MAGIC_FINGERS,
			"Magic fingers",
			"For the next 10 seconds, multiplies the current fire rate by 2 (delays any new increase until the end of the effect)",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.MagicFingers
		);
		AddRollEffect(
			RollEffectType.I_WENT_TO_THE_SHOOTING_RANGE,
			"I went to the shooting range!",
			"Increases the damage output by 0.5",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.IWentToTheShootingRange
		);
		AddRollEffect(
			RollEffectType.AMERICAN_SNIPER,
			"American Sniper",
			"Increases the damage output by 1",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.AmericanSniper
		);
		AddRollEffect(
			RollEffectType.FEAR_OF_DAMAGING_GOODS,
			"Fear of damaging goods",
			"Decrease the damage output by 0.5",
			EffectBehaviour.MALUS,
			RollEffectsDefinition.Instance.FearOfDamagingGoods
		);
		AddRollEffect(
			RollEffectType.EAGLE_EYE,
			"Eagle Eye",
			"For the next 10 seconds, multiplies the current damage ouput by 2 (delays any new increase until the end of the effect)",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.EagleEye
		);
		AddRollEffect(
			RollEffectType.TIME_TO_MAKE_PEACE,
			"Time to make peace.",
			"For the next 20 seconds, you cannot use your weapon",
			EffectBehaviour.MALUS,
			RollEffectsDefinition.Instance.TimeToMakePeace
		);
		AddRollEffect(
			RollEffectType.IS_THIS_MAGIC,
			"Is this magic?!",
			"For the next 10 seconds, the projectiles are seeking enemies (closest one)",
			EffectBehaviour.NEUTRAL,
			RollEffectsDefinition.Instance.IsThisMagic
		);
		AddRollEffect(
			RollEffectType.LOOK_AT_MY_NEW_GADGET,
			"Look at my new gadget!",
			"Transforms 1 projectile into a seeker",
			EffectBehaviour.NEUTRAL,
			RollEffectsDefinition.Instance.LookAtMyNewGadget
		);
		AddRollEffect(
			RollEffectType.DESTABILITATING_SHOTS,
			"Destabilitating shots",
			"Knockback +1",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.DestabilitatingShots
		);
		AddRollEffect(
			RollEffectType.DID_I_BUY_RUBBER_BULLETS,
			"Did I buy rubber bullets?!",
			"Knockback -1",
			EffectBehaviour.MALUS,
			RollEffectsDefinition.Instance.DidIBuyRubberBullets
		);
		AddRollEffect(
			RollEffectType.BULLET_HELL,
			"Bullet Hell!",
			"Projectile spread +1",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.BulletHell
		);
		AddRollEffect(
			RollEffectType.NOT_YOUR_GRANDPAS_AMMO,
			"Not your grandpa's ammo...",
			"Projectile size +10%",
			EffectBehaviour.BONUS,
			RollEffectsDefinition.Instance.NotYourGrandpasAmmo
		);

		// -------------------------------
		// ENEMY EVENTS

		// [TO DO] ...
	}
	private void AddRollEffect(RollEffectType type, string name, string description, EffectBehaviour behaviour, System.Action Activate)
	{
		RollEffectsAssociation.Add(type, new RollEffect(type, name, description, behaviour, Activate));
	}

	private void MapRollEffects()
	{
		AssociateRollEffects();

		// Roll Maps initialization
		AddRollMap(1, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.POWER_UP],
			RollEffectsAssociation[RollEffectType.ROLL_WITH_THE_FLOW],
			RollEffectsAssociation[RollEffectType.I_WENT_TO_THE_SHOOTING_RANGE],
			RollEffectsAssociation[RollEffectType.SACREBLUE_ITS_JAMMED_AGAIN],
			RollEffectsAssociation[RollEffectType.FEAR_OF_DAMAGING_GOODS],
			RollEffectsAssociation[RollEffectType.FEAR_OF_DAMAGING_GOODS]
		});

		AddRollMap(2, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.POWER_UP_PLUS],
			RollEffectsAssociation[RollEffectType.JUDGEMENT_DAY],
			RollEffectsAssociation[RollEffectType.ROLL_WITH_THE_FLOW],
			RollEffectsAssociation[RollEffectType.SACREBLUE_ITS_JAMMED_AGAIN],
			RollEffectsAssociation[RollEffectType.FEAR_OF_DAMAGING_GOODS],
			RollEffectsAssociation[RollEffectType.MAGIC_FINGERS]
		});

		AddRollMap(3, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.KILLING_FRENZY],
			RollEffectsAssociation[RollEffectType.NOT_TODAY],
			RollEffectsAssociation[RollEffectType.AMERICAN_SNIPER],
			RollEffectsAssociation[RollEffectType.JUST_NEEDED_SOME_GREASE],
			RollEffectsAssociation[RollEffectType.LOOK_AT_MY_NEW_GADGET],
			RollEffectsAssociation[RollEffectType.LOOK_AT_MY_NEW_GADGET]
		});

		AddRollMap(4, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.NOT_TODAY],
			RollEffectsAssociation[RollEffectType.POWER_UP],
			RollEffectsAssociation[RollEffectType.TIME_TO_MAKE_PEACE],
			RollEffectsAssociation[RollEffectType.I_WENT_TO_THE_SHOOTING_RANGE],
			RollEffectsAssociation[RollEffectType.LOOK_AT_MY_NEW_GADGET],
			RollEffectsAssociation[RollEffectType.JUST_NEEDED_SOME_GREASE]
		});

		AddRollMap(5, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.POWER_UP_PLUS],
			RollEffectsAssociation[RollEffectType.KILLING_FRENZY],
			RollEffectsAssociation[RollEffectType.FEAR_OF_DAMAGING_GOODS],
			RollEffectsAssociation[RollEffectType.EAGLE_EYE],
			RollEffectsAssociation[RollEffectType.MAGIC_FINGERS],
			RollEffectsAssociation[RollEffectType.TIME_TO_MAKE_PEACE]
		});

		AddRollMap(6, new List<RollEffect> {
			RollEffectsAssociation[RollEffectType.KILLING_FRENZY],
			RollEffectsAssociation[RollEffectType.JUDGEMENT_DAY],
			RollEffectsAssociation[RollEffectType.NOT_TODAY],
			RollEffectsAssociation[RollEffectType.SACREBLUE_ITS_JAMMED_AGAIN],
			RollEffectsAssociation[RollEffectType.JUST_NEEDED_SOME_GREASE],
			RollEffectsAssociation[RollEffectType.TIME_TO_MAKE_PEACE]
		});
	}
	private void AddRollMap(int id, List<RollEffect> effects)
	{
		RollMaps.Add(id, new RollMap(id) { Effects = effects });
	}

	public List<RollEffect> GetEffectsOnActivation(int result)
	{
		List<RollEffect> importedEffects = new List<RollEffect>();
		List<RollEffect> returnList = new List<RollEffect>();

		foreach (RollEffect rollEffect in RollMaps[result].Effects)
			importedEffects.Add(rollEffect);

		// We imply that there is at least 3 effects in the imported list (RollMaps[result].Effects)
		for (int i = 0; i < 3; i++)
		{
			int random = Random.Range(0, importedEffects.Count - 1);

			returnList.Add(importedEffects[random]);
			importedEffects.RemoveAt(random);
		}

		return returnList;
	}

	#region MAPPING
	public class RollMap
	{
		public int id;
		public List<RollEffect> Effects { get; set; }

		public RollMap(int id)
		{
			this.id = id;
		}
	}
	#endregion
}

public class RollEffect
{
	public int id;
	public string name;
	public string description;
	public EffectBehaviour behaviour;
	public System.Action Activate;

	public RollEffect(RollEffectType type, string name, string description, EffectBehaviour behaviour, System.Action Activate)
	{
		this.id = (int)type;
		this.name = name;
		this.description = description;
		this.behaviour = behaviour;
		this.Activate = Activate;
	}
}

public enum EffectBehaviour
{
	BONUS,
	MALUS,
	NEUTRAL
}

public enum RollEffectType
{
	POWER_UP,                               // OK
	POWER_UP_PLUS,                          // OK
	KILLING_FRENZY,                         // OK
	JUDGEMENT_DAY,                          // OK
	NOT_TODAY,                              // OK
	ROLL_WITH_THE_FLOW,                     // OK
	JUST_NEEDED_SOME_GREASE,                // OK
	SACREBLUE_ITS_JAMMED_AGAIN,             // OK
	MAGIC_FINGERS,                          // OK
	I_WENT_TO_THE_SHOOTING_RANGE,           // OK
	AMERICAN_SNIPER,                        // OK
	FEAR_OF_DAMAGING_GOODS,                 // OK
	EAGLE_EYE,                              // OK
	TIME_TO_MAKE_PEACE,                     // OK
	IS_THIS_MAGIC,
	LOOK_AT_MY_NEW_GADGET,                  // OK
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDataManager : MonoBehaviour
{
	#region BASE DATA: ENEMIES
	[System.Serializable] public class BaseData_Enemy
	{
		public float health;
		public float speed;
		public int damage;
		public int scoreWhenKilled;
		public float rotationalSpeed;

		public virtual void UpdateData(float health, float speed, int damage, int scoreWhenKilled, float rotationalSpeed)
		{
			this.health = health;
			this.speed = speed;
			this.damage = damage;
			this.scoreWhenKilled = scoreWhenKilled;
			this.rotationalSpeed = rotationalSpeed;
		}
	}

	[System.Serializable] public class BaseData_Enemy_MeleeDetonator : BaseData_Enemy { }
	[System.Serializable] public class BaseData_Enemy_DieHolder : BaseData_Enemy { }
	[System.Serializable] public class BaseData_Enemy_Pulsar : BaseData_Enemy
	{
		[Space]
		public float chargeDuration;
		public float holdDuration;
		public float blastDuration;
		public float cooldownDuration;

		public void UpdateData(float health, float speed, int damage, int scoreWhenKilled, float rotationalSpeed, float chargeDuration, float holdDuration, float blastDuration, float cooldownDuration)
		{
			this.health = health;
			this.speed = speed;
			this.damage = damage;
			this.scoreWhenKilled = scoreWhenKilled;
			this.rotationalSpeed = rotationalSpeed;
			this.chargeDuration = chargeDuration;
			this.holdDuration = holdDuration;
			this.blastDuration = blastDuration;
			this.cooldownDuration = cooldownDuration;
		}
	}

	public static BaseData_Enemy_MeleeDetonator baseDataEnemy_MeleeDetonator;
	public static BaseData_Enemy_DieHolder baseDataEnemy_DieHolder;
	public static BaseData_Enemy_Pulsar baseDataEnemy_Pulsar;

	[Header("BASE DATA: ENEMIES")]
	[SerializeField] private BaseData_Enemy_MeleeDetonator _baseDataEnemy_MeleeDetonator;
	[SerializeField] private BaseData_Enemy_DieHolder _baseDataEnemy_DieHolder;
	[SerializeField] private BaseData_Enemy_Pulsar _baseDataEnemy_Pulsar;
	#endregion

	#region BASE DATA: PLAYER
	[System.Serializable] public class BaseData_Player
	{
		public int health;
		public int speed;
		public int damage;
		public int firerateStandard;
		public int firerateSeeker;

		public void UpdateData(int health, int speed, int damage, int firerateStandard, int firerateSeeker)
		{
			this.health = health;
			this.speed = speed;
			this.damage = damage;
			this.firerateStandard = firerateStandard;
			this.firerateSeeker = firerateSeeker;
		}
	}

	public static BaseData_Player baseDataPlayer;

	[Header("BASE DATA: PLAYER")]
	[SerializeField] private BaseData_Player _baseDataPlayer;
	#endregion

	#region BASE DATA: PROJECTILES
	[System.Serializable] public class BaseData_Projectile
	{
		public float speed;
		public float damage;
		public float timeAlive; 
	}

	[System.Serializable] public class BaseData_Projectile_Standard : BaseData_Projectile { }
	[System.Serializable] public class BaseData_Projectile_Explosive : BaseData_Projectile
	{
		[Space]
		public float blastDamage;
		public float blastRadius;
	}
	[System.Serializable] public class BaseData_Projectile_Ricochet : BaseData_Projectile
	{
		[Space]
		public int maxRicochet;
	}
	[System.Serializable] public class BaseData_Projectile_Seeker : BaseData_Projectile
	{
		[Space]
		public float angularVelocity;
	}

	public static BaseData_Projectile_Standard baseDataProjectile_Standard;
	public static BaseData_Projectile_Explosive baseData_Projectile_Explosive;
	public static BaseData_Projectile_Ricochet baseDataProjectile_Ricochet;
	public static BaseData_Projectile_Seeker baseDataProjectile_Seeker;

	[Header("BASE DATA: PROJECTILES")]
	[SerializeField] private BaseData_Projectile_Standard _baseDataProjectile_Standard;
	[SerializeField] private BaseData_Projectile_Explosive _baseDataProjectile_Explosive;
	[SerializeField] private BaseData_Projectile_Ricochet _baseDataProjectile_Ricochet;
	[SerializeField] private BaseData_Projectile_Seeker _baseDataProjectile_Seeker;
	#endregion

	#region INITIALIZATION
	private void Awake()
	{
		baseDataEnemy_MeleeDetonator = _baseDataEnemy_MeleeDetonator;
		baseDataEnemy_DieHolder = _baseDataEnemy_DieHolder;
		baseDataEnemy_Pulsar = _baseDataEnemy_Pulsar;

		baseDataPlayer = _baseDataPlayer;

		baseDataProjectile_Standard = _baseDataProjectile_Standard;
		baseData_Projectile_Explosive = _baseDataProjectile_Explosive;
		baseDataProjectile_Ricochet = _baseDataProjectile_Ricochet;
		baseDataProjectile_Seeker = _baseDataProjectile_Seeker;
	}
	#endregion
}
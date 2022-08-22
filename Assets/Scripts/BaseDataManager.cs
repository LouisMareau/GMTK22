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

	#region INITIALIZATION
	private void Awake()
	{
		baseDataEnemy_MeleeDetonator = _baseDataEnemy_MeleeDetonator;
		baseDataEnemy_DieHolder = _baseDataEnemy_DieHolder;
		baseDataEnemy_Pulsar = _baseDataEnemy_Pulsar;

		baseDataPlayer = _baseDataPlayer;
	}
	#endregion
}
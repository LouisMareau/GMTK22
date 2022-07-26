using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSeeker : Projectile
{
	[Header("SEEKING")]
	[SerializeField] private float _angularVelocity = 3f;

	protected override void Update()
	{
		// We find the closest enemy and update the direction
		Enemy target = EnemySpawner.Instance.FindClosestEnemy(_rootTransform.position);

		if (target != null)
			_direction = Vector3.RotateTowards(_direction, (target.targetTransform.position - _rootTransform.position), MathUtils.GetRadiansFromDegrees(_angularVelocity) , 0).normalized;

		base.Update();
	}
}

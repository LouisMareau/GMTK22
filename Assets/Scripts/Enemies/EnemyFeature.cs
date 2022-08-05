using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature : MonoBehaviour
{
	[SerializeField] protected Enemy _associatedEnemy;
	
	protected Transform _rootTransform;

	#region INITIALIZATION
	protected virtual void Awake()
	{
		if (_associatedEnemy == null) { throw new System.NullReferenceException("The associatedEnemy member cannot be null."); }

		_rootTransform = transform;
	}
	#endregion
}
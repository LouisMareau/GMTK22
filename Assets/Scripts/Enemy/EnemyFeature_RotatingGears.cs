using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature_RotatingGears : MonoBehaviour
{
	[Header("DATA")]
	[SerializeField] private float minRotationSpeed = 0f;
	[SerializeField] private float maxRotationSpeed = 10f;

	private Transform _rootTransform;

	private void Awake()
	{
		_rootTransform = transform;
	}

	public void StartCharging(float duration) { StartCoroutine(StartCharging_Coroutine(duration)); }
	private IEnumerator StartCharging_Coroutine(float duration)
	{
		float timer = 0;
		while (timer < duration)
		{
			float angle = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, timer / duration);
			_rootTransform.Rotate(Vector3.right, angle);

			timer += Time.deltaTime;
			yield return null;
		}
	}
}
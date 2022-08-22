using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeature_EnergyRing : MonoBehaviour
{
	[Header("DATA")]
	[SerializeField] private Vector3 _siegePositionOffset = new Vector3(0.0f, 0.5f, 0.0f);
	[Space]
	[SerializeField] private float _startEnergyIntensity = 0.1f;
	[SerializeField] private float _endEnergyIntensity = 2.0f;

	[Header("VFXs")]
	[SerializeField] private GameObject _prefabVFX_siegeDustClouds;

	private Transform _rootTransform;
	private Vector3 _originPosition;
	private Material _energyMaterial;
	private Color _energyColor;

	#region INITIALIZATION
	private void Awake()
	{
		_rootTransform = transform;
		_originPosition = _rootTransform.position;
		_energyMaterial = GetComponent<MeshRenderer>().materials[1];
		_energyColor = _energyMaterial.GetColor("_EmissionColor");
	}
	#endregion

	public void StartSieging(float duration) { StartCoroutine(StartSieging_Coroutine(duration)); }
	private IEnumerator StartSieging_Coroutine(float duration)
	{
		_originPosition = _rootTransform.position;

		float timer = 0.0f;
		while (timer < duration)
		{
			_rootTransform.position = Vector3.Lerp(_originPosition, _originPosition - _siegePositionOffset, timer / duration);

			float intensity = Mathf.Lerp(_startEnergyIntensity, _endEnergyIntensity, timer / duration);
			float factor = Mathf.Pow(2.0f, intensity * Random.Range(0.92f, 1.08f));
			_energyMaterial.SetColor("_EmissionColor", new Color(_energyColor.r * factor, _energyColor.g * factor, _energyColor.b * factor));

			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		PlayAnim_SiegeDustClouds();
	}

	public void StopSieging(float duration) { StartCoroutine(StopSieging_Coroutine(duration)); }
	private IEnumerator StopSieging_Coroutine(float duration)
	{
		float timer = 0.0f;
		while (timer < duration)
		{
			_rootTransform.position = Vector3.Lerp(_originPosition - _siegePositionOffset, _originPosition, timer / duration);

			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}
	}

	public void StartEnergyFlow(float duration) { StartCoroutine(StartEnergyFlow_Coroutine(duration)); }
	private IEnumerator StartEnergyFlow_Coroutine(float duration)
	{
		float timer = 0.0f;
		while (timer < duration)
		{
			float intensity = Mathf.Lerp(_startEnergyIntensity, _endEnergyIntensity, timer / duration);
			float factor = Mathf.Pow(2.0f, intensity * Random.Range(0.92f, 1.08f));
			_energyMaterial.SetColor("_EmissionColor", new Color(_energyColor.r * factor, _energyColor.g * factor, _energyColor.b * factor));

			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}
	}

	public void StopEnergyFlow(float duration) { StartCoroutine(StopEnergyFlow_Coroutine(duration)); }
	private IEnumerator StopEnergyFlow_Coroutine(float duration)
	{
		float timer = 0.0f;
		while (timer < duration)
		{
			float intensity = Mathf.Lerp(_endEnergyIntensity, _startEnergyIntensity, timer / (duration / 3f));
			float factor = Mathf.Pow(2, intensity);
			_energyMaterial.SetColor("_EmissionColor", new Color(_energyColor.r * factor, _energyColor.g * factor, _energyColor.b * factor));

			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}
	}

	#region ANIMATIONS / VFX
	protected virtual void PlayAnim_SiegeDustClouds()
	{
		// VFX: Siege Dust Clouds
		Instantiate(_prefabVFX_siegeDustClouds, _rootTransform.position, _prefabVFX_siegeDustClouds.transform.rotation, StaticReferences.Instance.vfxContainer);
	}
	#endregion
}
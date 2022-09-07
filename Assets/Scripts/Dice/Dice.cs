using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dice : MonoBehaviour
{
	public int Result { get; protected set; }

	protected DiceHUD _menu;

	[SerializeField] protected Vector2 _idleMinMaxRotationSpeed = new Vector2(5f, 10f);
	protected Vector3 _idleRandomEulerAngle;

	public new Rigidbody rigidbody;
	[SerializeField] protected Transform _rootTransform;
	[SerializeField] protected Transform _meshTransform;
	[SerializeField] protected Transform _menuTransform;

	#region INITIALIZATION
	protected virtual void Awake()
	{
		if (rigidbody == null) { rigidbody = GetComponent<Rigidbody>(); }
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.Find("Mesh"); }
		if (_menuTransform == null) { _menuTransform = _rootTransform.Find("Menu"); }
		if (_menu == null) { _menu = DiceHUD.Instance; }
	}

	protected virtual void Start()
	{
		rigidbody.isKinematic = true;

		_idleRandomEulerAngle = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * Random.Range(_idleMinMaxRotationSpeed.x, _idleMinMaxRotationSpeed.y);
	}
	#endregion

	protected virtual void Update()
	{
		if (GameManager.IsPlaying)
		{
			if (rigidbody.isKinematic)
				_rootTransform.Rotate(_idleRandomEulerAngle * Time.deltaTime);
		}
	}

	#region GAMEPLAY
	public void Activate()
	{
		if (Result != 0)
		{
			// We pause the game
			GameManager.SwitchGameState(GameState.PAUSE);

			_menu.ActivateDice(Result);
		}
	}

	protected void ActivateRandomEffect()
	{
		List<RollEffect> effects = DiceManager.Effects.GetEffectsOnActivation(Result);
		int randomIndex = Random.Range(0, effects.Count - 1);
		effects[randomIndex].Activate();

		// We notify the player
		DiceHUD.Instance.Notify(effects[randomIndex]);
	}

	public virtual void Kill()
	{
		// VFX: Death
		// [TO DO]

		Destroy(gameObject);
	}

	public void KillAfterDelay(float delay = 12.0f) { StartCoroutine(KillAfterDelay_Coroutine(delay)); }
	private IEnumerator KillAfterDelay_Coroutine(float delay)
	{
		float timer = 0.0f;
		while (timer < delay)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		ActivateRandomEffect();

		Kill();
	}
	#endregion

	#region COLLISION EVENTS
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			_menu.SetAssociatedDice(this);
			Activate();
		}
	}
	#endregion

	#region FACE-LANDING DETECTION
	[Header("FACE-LANDING DETECTION PARAMETERS")]
	[SerializeField] protected float _rayDetectionLength = 2f;

	protected Ray CastRayToLocalDirection(Vector3 direction, float length)
	{
		return new Ray(_meshTransform.position, (_meshTransform.rotation * direction).normalized * length);
	}

	protected abstract void CalculateDetectionRays();
	protected abstract void DetectResult();
	#endregion
}

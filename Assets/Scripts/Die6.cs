using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die6 : Die
{
	public int Result { get; private set; }

	private DieMenu _menu;

	[SerializeField] private Vector2 _idleMinMaxRotationSpeed = new Vector2(5f, 10f);

	public new Rigidbody rigidbody;

	[SerializeField] private Transform _rootTransform;
	[SerializeField] private Transform _meshTransform;
	[SerializeField] private Transform _menuTransform;

	private Ray _detectionRaySide1;
	private Ray _detectionRaySide2;
	private Ray _detectionRaySide3;
	private Ray _detectionRaySide4;
	private Ray _detectionRaySide5;
	private Ray _detectionRaySide6;

	private Vector3 _idleRandomEulerAngle;

	#region INTIALIZATION
	private void Awake()
	{
		if (rigidbody == null) { rigidbody = GetComponent<Rigidbody>(); }
		if (_rootTransform == null) { _rootTransform = transform; }
		if (_meshTransform == null) { _meshTransform = _rootTransform.Find("Mesh"); }
		if (_menuTransform == null) { _menuTransform = _rootTransform.Find("Menu"); }
		if (_menu == null) { _menu = DieMenu.Instance; }
	}

	private void Start()
	{
		if (_menu.IsVisible)
			_menu.Hide();

		rigidbody.isKinematic = true;

		_idleRandomEulerAngle = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * Random.Range(_idleMinMaxRotationSpeed.x, _idleMinMaxRotationSpeed.y);
	}
	#endregion

	protected virtual void Update()
	{
		if (GameManager.IsPlaying)
		{
			if (rigidbody.isKinematic)
			{
				_rootTransform.Rotate(_idleRandomEulerAngle * Time.deltaTime);
			}
			else
			{
				_detectionRaySide1 = CastRayToLocalDirection(Vector3.down, 2f);
				_detectionRaySide2 = CastRayToLocalDirection(Vector3.right, 2f);
				_detectionRaySide3 = CastRayToLocalDirection(Vector3.forward, 2f);
				_detectionRaySide4 = CastRayToLocalDirection(Vector3.back, 2f);
				_detectionRaySide5 = CastRayToLocalDirection(Vector3.left, 2f);
				_detectionRaySide6 = CastRayToLocalDirection(Vector3.up, 2f);

				if (Physics.Raycast(_detectionRaySide1, LayerMask.GetMask("Floor")))
					Result = 1;
				else if (Physics.Raycast(_detectionRaySide2, LayerMask.GetMask("Floor")))
					Result = 2;
				else if (Physics.Raycast(_detectionRaySide3, LayerMask.GetMask("Floor")))
					Result = 3;
				else if (Physics.Raycast(_detectionRaySide4, LayerMask.GetMask("Floor")))
					Result = 4;
				else if (Physics.Raycast(_detectionRaySide5, LayerMask.GetMask("Floor")))
					Result = 5;
				else if (Physics.Raycast(_detectionRaySide6, LayerMask.GetMask("Floor")))
					Result = 6;
			}
		}
	}

	private Ray CastRayToLocalDirection(Vector3 direction, float length)
	{
		return new Ray(_meshTransform.position, (_meshTransform.rotation * direction).normalized * length);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
            _menu.SetAssociatedDie(this);
			_menu.Activate(Result);
        }
	}

	public void Kill()
	{
		// VFX: Death
		// [TO DO]

		Destroy(gameObject);
	}
	public void KillAfterDelay(float delay = 20f) { StartCoroutine(KillAfterDelay_Coroutine(delay)); }
	private IEnumerator KillAfterDelay_Coroutine(float delay)
	{
		float timer = 0.0f;
		while (timer < delay)
		{
			if (GameManager.IsPlaying)
				timer += Time.deltaTime;

			yield return null;
		}

		Kill();
	}
}

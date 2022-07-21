using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

	private Transform _rootTransform;
	private Transform _cameraTransform;

	private void Awake()
	{
		_rootTransform = transform;
		_cameraTransform = _rootTransform.GetChild(0);
	}

	private void LateUpdate()
	{
		Vector3 designredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(_rootTransform.position, designredPosition, smoothSpeed);
		_rootTransform.position = smoothedPosition;
	}
}

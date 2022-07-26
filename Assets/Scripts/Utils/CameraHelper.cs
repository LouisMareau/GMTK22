using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    [SerializeField] private GameObject _prefabHelper;

	[SerializeField] private bool _shouldShowHelpersOnStart = false;

    private Camera _camera;

    private static Vector3 _onScreenFloorTLCorner;
    private static Vector3 _onScreenFloorTRCorner;
    private static Vector3 _onScreenFloorBLCorner;
    private static Vector3 _onScreenFloorBRCorner;
    private static Vector3 _onScreenFloorLeft;
    private static Vector3 _onScreenFloorRight;
    private static Vector3 _onScreenFloorTop;
    private static Vector3 _onScreenFloorBottom;


	#region INITIALIZATION
	private void Awake()
	{
		_camera = Camera.main;

		_onScreenFloorTLCorner = _camera.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height, 120f));
		_onScreenFloorTRCorner = _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 120f));
		_onScreenFloorBLCorner = _camera.ScreenToWorldPoint(new Vector3(0f, 0f, 120f));
		_onScreenFloorBRCorner = _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, 0f, 120f));
		_onScreenFloorLeft = _camera.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height / 2f, 120f));
		_onScreenFloorRight = _camera.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height / 2f, 120f));
		_onScreenFloorTop = _camera.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, (float)Screen.height, 120f));
		_onScreenFloorBottom = _camera.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, 0f, 120f));
	}

	private void Start()
	{
		if (_shouldShowHelpersOnStart)
			InstantiateHelpers();
	}
	#endregion

	public void InstantiateHelpers()
	{
		Instantiate(_prefabHelper, _onScreenFloorTLCorner, Quaternion.identity, StaticReferences.Instance.miscs).name = "TL CORNER";
		Instantiate(_prefabHelper, _onScreenFloorTRCorner, Quaternion.identity, StaticReferences.Instance.miscs).name = "TR CORNER";
		Instantiate(_prefabHelper, _onScreenFloorBLCorner, Quaternion.identity, StaticReferences.Instance.miscs).name = "BL CORNER";
		Instantiate(_prefabHelper, _onScreenFloorBRCorner, Quaternion.identity, StaticReferences.Instance.miscs).name = "BR CORNER";
		Instantiate(_prefabHelper, _onScreenFloorLeft, Quaternion.identity, StaticReferences.Instance.miscs).name = "LEFT";
		Instantiate(_prefabHelper, _onScreenFloorRight, Quaternion.identity, StaticReferences.Instance.miscs).name = "RIGHT";
		Instantiate(_prefabHelper, _onScreenFloorTop, Quaternion.identity, StaticReferences.Instance.miscs).name = "TOP";
		Instantiate(_prefabHelper, _onScreenFloorBottom, Quaternion.identity, StaticReferences.Instance.miscs).name = "BOTTOM";
	}

	public static float GetEdgeLeft() { return _onScreenFloorLeft.x; }
	public static float GetEdgeRight() { return _onScreenFloorRight.x; }
	public static float GetEdgeTop() { return _onScreenFloorTop.z; }
	public static float GetEdgeBottom() { return _onScreenFloorBottom.z; }
}

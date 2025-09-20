using UnityEngine;

public class CameraManager : MonoBehaviour
{
	//private int fingerId = -1;

	private Vector2 fingerTouchStartPosition;
	private float fingerTouchStartTime;

	//private float dragDistance = 0.2f;

	private float cameraDistance;
	private Vector3 origin = Vector3.zero;

	//public float rotationYSpeed = 120f;
	//public float rotationXSpeed = 60f;

	public float zoomSpeed = 100f;
	public float moveSpeed = 0.2f;

	private float cameraMaxDistance = 33f;
	private float cameraMinDistance = 15f;
	//private bool isDrag;

	public GameObject chessboard;

	private Vector3 CameraDeckSettingInitPos;
	private Vector3 CameraDeckSettingGamePos;

	private Vector2 startTouch1;
	private Vector2 startTouch2;
	private bool isStartDoubleTouch = false;
	
	private Vector3 initOffset = new Vector3(-13, 20, -5);
	private Vector3 gameOffset = new Vector3(10, 33, 0);

	void Start()
    {
		//isDrag = false;
		CameraDeckSettingGamePos = chessboard.transform.position + gameOffset;
		CameraDeckSettingInitPos = chessboard.transform.position + initOffset;
		Camera.main.transform.position = CameraDeckSettingInitPos;
		cameraDistance = transform.position.y - chessboard.transform.position.y;
	}

    void Update()
    {
        var touches = Input.touches;
  //      foreach (Touch touch in touches)
  //      {
		//	switch (touch.phase)
		//	{
		//		case TouchPhase.Began:
		//			if (fingerId == -1)
		//			{
		//				fingerId = touch.fingerId;
		//				fingerTouchStartPosition = touch.position;
		//				fingerTouchStartTime = Time.time;
		//			}
		//			break;
		//		case TouchPhase.Moved:
		//			if(touch.fingerId == fingerId && touches.Length == 1)
		//			{
		//				//Debug.Log((touch.position - fingerTouchStartPosition).magnitude / Screen.dpi);
		//				if (((touch.position - fingerTouchStartPosition).magnitude / Screen.dpi < dragDistance))
		//				{
		//					if(!isDrag)
		//						return;
		//				}

		//				var direction = touch.deltaPosition;

		//				isDrag = true;
		//				direction.Normalize();

		//				var rotation = transform.localRotation.eulerAngles;
		//				rotation.x = Mathf.Clamp(rotation.x + direction.y * rotationYSpeed * Time.deltaTime, 30, 90);
		//				transform.localRotation = Quaternion.Euler(rotation);

		//				transform.Rotate(Vector3.up, direction.x * rotationXSpeed * Time.deltaTime, Space.World);

		//				transform.position = cameraDistance * -transform.forward;
		//			}
		//			break;
		//		case TouchPhase.Stationary:

		//			break;
		//		case TouchPhase.Ended:
		//		case TouchPhase.Canceled:
		//			isDrag = false;
		//			fingerId = -1;
		//			break;
		//	}
		//}

		if(Input.touchCount == 2)
		{

			if (!isStartDoubleTouch)
			{
				isStartDoubleTouch = true;

				startTouch1 = Input.GetTouch(0).position;
				startTouch2 = Input.GetTouch(1).position;
			}

			var startDistance = Vector2.Distance(startTouch1, startTouch2);
			
			var touch1 = Input.GetTouch(0);
			var touch2 = Input.GetTouch(1);

			float currentDistance = Vector2.Distance(touch1.position, touch2.position);

			Vector2 touch1beforePos = touch1.position - touch1.deltaPosition;
			Vector2 touch2beforePos = touch2.position - touch2.deltaPosition;
			float beforeDistance = Vector2.Distance(touch1beforePos, touch2beforePos);

			float delta = (currentDistance - beforeDistance) / Screen.dpi;
			float startDelta = (currentDistance - startDistance) / Screen.dpi;
	
			if(Mathf.Abs(startDelta) < 0.2f)
			{
				Vector2 beforeCenter = (touch1beforePos + touch2beforePos) / 2f;
				Vector2 currentCenter = (touch1.position + touch2.position) / 2f;

				var dir = (currentCenter - beforeCenter);
				//var dir3D = new Vector3(-Mathf.Clamp(dir.y, -1f,1f), 0, -Mathf.Clamp(-dir.x, -1f, 1f));

				transform.Translate(new Vector3(-dir.x, -dir.y, 0) * Time.deltaTime * moveSpeed);
			}
			else
			{
				cameraDistance = Mathf.Clamp(cameraDistance - delta * zoomSpeed * Time.deltaTime, cameraMinDistance, cameraMaxDistance);
				var vec = transform.position;

				vec.y = chessboard.transform.position.y + cameraDistance;
				transform.position = vec;
			}
		}
		else
		{
			isStartDoubleTouch = false;
		}
	}

	public void SetCameraToSettingPos()
	{
		Camera.main.transform.position = CameraDeckSettingInitPos;
	}
}

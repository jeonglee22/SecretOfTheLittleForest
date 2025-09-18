using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private int fingerId = -1;

	private Vector2 fingerTouchStartPosition;
	private float fingerTouchStartTime;

	private float dragDistance = 0.2f;

	private float cameraDistance;
	private Vector3 origin = Vector3.zero;

	public float rotationYSpeed = 120f;
	public float rotationXSpeed = 60f;

	public float zoomSpeed = 100f;

	private float cameraMaxDistance = 33f;
	private float cameraMinDistance = 20f;
	private bool isDrag;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		cameraDistance = transform.position.magnitude;
		isDrag = false;
    }

    // Update is called once per frame
    void Update()
    {
        var touches = Input.touches;
        foreach (Touch touch in touches)
        {
			switch (touch.phase)
			{
				case TouchPhase.Began:
					if (fingerId == -1)
					{
						fingerId = touch.fingerId;
						fingerTouchStartPosition = touch.position;
						fingerTouchStartTime = Time.time;
					}
					break;
				case TouchPhase.Moved:
					if(touch.fingerId == fingerId && touches.Length == 1)
					{
						//Debug.Log((touch.position - fingerTouchStartPosition).magnitude / Screen.dpi);
						if (((touch.position - fingerTouchStartPosition).magnitude / Screen.dpi < dragDistance))
						{
							if(!isDrag)
								return;
						}

						var direction = touch.deltaPosition;

						isDrag = true;
						direction.Normalize();

						var rotation = transform.localRotation.eulerAngles;
						rotation.x = Mathf.Clamp(rotation.x + direction.y * rotationYSpeed * Time.deltaTime, 30, 90);
						transform.localRotation = Quaternion.Euler(rotation);

						transform.Rotate(Vector3.up, direction.x * rotationXSpeed * Time.deltaTime, Space.World);

						transform.position = cameraDistance * -transform.forward;
					}
					break;
				case TouchPhase.Stationary:

					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					isDrag = false;
					fingerId = -1;
					break;
			}
		}

		if(Input.touchCount == 2)
		{
			var touch1 = Input.GetTouch(0);
			var touch2 = Input.GetTouch(1);

			float currentDistance = Vector2.Distance(touch1.position, touch2.position);

			Vector2 touch1beforePos = touch1.position - touch1.deltaPosition;
			Vector2 touch2beforePos = touch2.position - touch2.deltaPosition;
			float beforeDistance = Vector2.Distance(touch1beforePos, touch2beforePos);

			float delta = (currentDistance - beforeDistance) / Screen.dpi;
			delta *= Time.deltaTime * zoomSpeed;

			cameraDistance -= delta;
			cameraDistance = Mathf.Clamp(cameraDistance, cameraMinDistance, cameraMaxDistance);

			transform.position = cameraDistance * -transform.forward;
		}
	}
}

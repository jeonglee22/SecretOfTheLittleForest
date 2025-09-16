using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
	private int fingerId = -1;

	private Vector2 fingerTouchStartPosition;
	private float fingerTouchStartTime;

	private float sweepDistance = 1f;
	private float tapTimeLimit = 0.1f;
	private float holdTimeLimit = 0.1f;

	public static float FingersDelta { get; private set; }
	public static bool IsTap { get; private set; }
	public static bool IsLongPress { get; private set; }

	private float deltaOffset = 5f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		var touches = Input.touches;
		float timeDiff = -1f;
		float disDiff = -1;
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
					if (touch.fingerId == fingerId && touches.Length == 1)
					{
					}
					break;
				case TouchPhase.Stationary:

					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					timeDiff = Time.time - fingerTouchStartTime;
					disDiff = Vector2.Distance(touch.position, fingerTouchStartPosition);
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

			FingersDelta = (currentDistance - beforeDistance) / Screen.dpi;
			FingersDelta *= Time.deltaTime * deltaOffset;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		fingerTouchStartTime = Time.time;
		fingerTouchStartPosition = eventData.position;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		var timeDiff = Time.time - fingerTouchStartTime;
		var disDiff = Vector2.Distance(eventData.position, fingerTouchStartPosition);

		if (timeDiff <= tapTimeLimit)
			IsTap = true;
		else if (timeDiff <= holdTimeLimit)
		{
			IsLongPress = true;
			IsTap = false;

		}
	}

	public void OnPointerMove(PointerEventData eventData)
	{
	}
}
